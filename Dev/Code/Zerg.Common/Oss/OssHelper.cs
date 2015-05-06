using System;
using System.Collections.Generic;
using System.IO;
using Aliyun.OpenServices.OpenStorageService;

namespace Zerg.Common.Oss
{
    public class OssHelper
    {
        public const string AccessId = "91a6ZJyoBlBNrJnH";
        public const string AccessKey = "yUGDJHYudo1DkE71PMOIc4bgAqVipa";
        public const string Endpoint = "http://oss-cn-hangzhou.aliyuncs.com/";
        public const string BucketName = "chuangfubao";
        private static readonly OssClient OssClient = new OssClient(Endpoint, AccessId, AccessKey);
        
        public static string CreateEmptyFolder()
        {


            var client = OssClient;


            // Note: key treats as a folder and must end with slash.
            const string key = "yourfolder/";
            var created = false;
            var uploaded = false;
            try
            {
                // create bucket
                client.CreateBucket(BucketName);
                created = true;

                // put object with zero bytes stream.
                using (MemoryStream memStream = new MemoryStream())
                {
                    PutObjectResult ret = client.PutObject(BucketName, key, memStream);
                    uploaded = true;
                    return ret.ETag;
                }
            }
            catch (OssException ex)
            {
                if (ex.ErrorCode == OssErrorCode.BucketAlreadyExists)
                {
                    return  string.Format("Bucket '{0}' already exists, please modify and recreate it.", BucketName);
                }
                else
                {
                    return  string.Format("CreateBucket Failed with error info: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                }
            }
            finally
            {
                if (uploaded)
                {
                    client.DeleteObject(BucketName, key);
                }
                if (created)
                {
                    client.DeleteBucket(BucketName);
                }
            }
        }

        public static void DeleteObjects()
        {
            var client = new OssClient(Endpoint, AccessId, AccessKey);

            try
            {
                var keys = new List<string>();
                var listResult = client.ListObjects(BucketName);
                foreach (var summary in listResult.ObjectSummaries)
                {
                    Console.WriteLine(summary.Key);
                    keys.Add(summary.Key);
                }
                var request = new DeleteObjectsRequest(BucketName, keys, false);
                client.DeleteObjects(request);
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }

        /// <summary>
        /// 分片上传。
        /// </summary>
        /// <param name="bucketName"><see cref="chuangfubao"/></param>
        /// <param name="objectName"><see cref="OssObject"/></param>
        /// <param name="fileToUpload">指定分片上传文件路径</param>
        /// <param name="partSize">分片大小（单位：字节）</param>
        public static string UploadMultipart(String bucketName, String objectName, String fileToUpload, int partSize)
        {
            var uploadId = InitiateMultipartUpload(bucketName, objectName);
            var partETags = BeginUploadPart(bucketName, objectName, fileToUpload, uploadId, partSize);
            var completeResult = CompleteUploadPart(bucketName, objectName, uploadId, partETags);
            return completeResult.Location;
        }

        /// <summary>
        /// 分片拷贝。
        /// </summary>
        /// <param name="targetBucket">目标<see cref="Bucket"/></param>
        /// <param name="targetKey">目标<see cref="OssObject"/></param>
        /// <param name="sourceBucket">源<see cref="Bucket"/></param>
        /// <param name="sourceKey">源<see cref="OssObject"/></param>
        /// <param name="partSize">分片大小（单位：字节）</param>
        public static void UploadMultipartCopy(String targetBucket, String targetKey, String sourceBucket, String sourceKey, int partSize)
        {
            var uploadId = InitiateMultipartUpload(targetBucket, targetKey);
            var partETags = BeginUploadPartCopy(targetBucket, targetKey, sourceBucket, sourceKey, uploadId, partSize);
            var completeResult = CompleteUploadPart(targetBucket, targetKey, uploadId, partETags);

            Console.WriteLine("Upload multipart copy result : ");
            Console.WriteLine(completeResult.Location);
        }

        private static string InitiateMultipartUpload(String bucketName, String objectName)
        {
            var request = new InitiateMultipartUploadRequest(bucketName, objectName);
            var result = OssClient.InitiateMultipartUpload(request);
            return result.UploadId;
        }

        private static List<PartETag> BeginUploadPart(String bucketName, String objectName, String fileToUpload,
            String uploadId, int partSize)
        {
            var fileSize = -1;
            using (var fs = File.Open(fileToUpload, FileMode.Open))
            {
                fileSize = (int)fs.Length;
            }

            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            for (var i = 0; i < partCount; i++)
            {
                using (FileStream fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var skipBytes = (long)partSize * i;
                    fs.Seek(skipBytes, 0);
                    var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                    var request = new UploadPartRequest(bucketName, objectName, uploadId);
                    request.InputStream = fs;
                    request.PartSize = size;
                    request.PartNumber = i + 1;
                    var result = OssClient.UploadPart(request);
                    partETags.Add(result.PartETag);
                }
            }
            return partETags;
        }

        private static List<PartETag> BeginUploadPartCopy(String targetBucket, String targetKey, String sourceBucket, String sourceKey,
            String uploadId, int partSize)
        {
            var metadata = OssClient.GetObjectMetadata(sourceBucket, sourceKey);
            var fileSize = metadata.ContentLength;

            var partCount = (int)fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = (long)partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request =
                    new UploadPartCopyRequest(targetBucket, targetKey, sourceBucket, sourceKey, uploadId);
                request.PartSize = size;
                request.PartNumber = i + 1;
                request.BeginIndex = skipBytes;
                var result = OssClient.UploadPartCopy(request);
                partETags.Add(result.PartETag);
            }
            return partETags;
        }

        private static CompleteMultipartUploadResult CompleteUploadPart(String bucketName, String objectName,
            String uploadId, List<PartETag> partETags)
        {
            var completeMultipartUploadRequest =
                new CompleteMultipartUploadRequest(bucketName, objectName, uploadId);
            foreach (var partETag in partETags)
            {
                completeMultipartUploadRequest.PartETags.Add(partETag);
            }

            return OssClient.CompleteMultipartUpload(completeMultipartUploadRequest);
        }

        public static void PutObject(Stream fs,string fileName)
        {


            OssClient client = OssClient;

            string bucketName = BucketName;
            string key = fileName;
//            const string fileToUpload = "<file to upload>";
//
//            try
//            {
                // 1. put object to specified output stream
//                using (var fs = File.Open(fileToUpload, FileMode.Open))
//                {
            using (fs)
            {
                var metadata = new ObjectMetadata();
                metadata.UserMetadata.Add("mykey1", "myval1");
                metadata.UserMetadata.Add("mykey2", "myval2");
                metadata.CacheControl = "No-Cache";
                metadata.ContentType = "text/html";
                client.PutObject(bucketName, key, fs, metadata);

                metadata = client.GetObjectMetadata(bucketName, key);
            }

//                }

                // 2. put object to specified file
                //client.PutObject(bucketName, key, fileToUpload);

                // 3. put object from specified object with multi-level virtual directory
                //key = "folder/sub_folder/key0";
                //client.PutObject(bucketName, key, fileToUpload);
//            }
//            catch (OssException ex)
//            {
//                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
//                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Failed with error info: {0}", ex.Message);
//            }
        }
    }
}
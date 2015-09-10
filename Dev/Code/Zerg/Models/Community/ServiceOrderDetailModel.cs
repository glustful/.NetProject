namespace Zerg.Models.Community
{
	public class ServiceOrderDetailModel
	{

		/// <summary>
        /// Id
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// Order_Id
        /// </summary>
//		public ServiceOrderEntity ServiceOrder {get;set;}


		/// <summary>
        /// Product_Id
        /// </summary>
		public ProductModel Product {get;set;}


		/// <summary>
        /// Count
        /// </summary>
		public decimal Count {get;set;}


		/// <summary>
        /// Price
        /// </summary>
		public decimal Price {get;set;}



	}
}
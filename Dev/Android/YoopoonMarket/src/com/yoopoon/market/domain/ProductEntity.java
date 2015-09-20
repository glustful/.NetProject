package com.yoopoon.market.domain;

import java.io.Serializable;

public class ProductEntity implements Serializable {
	/**
	 * @fieldName: serialVersionUID
	 * @fieldType: long
	 * @Description: TODO
	 */
	private static final long serialVersionUID = 1L;
	public int Id = 1;
	public int BussnessId = 0;
	public String BussnessName = null;
	public float Price = 0;
	public String Name;
	public int Status = 0;
	public String MainImg = null;
	public int IsRecommend = 0;
	public int Sort = 0;
	public int Stock = 0;
	public int Adduser = 0;
	public String Addtime;
	public int UpdUser = 0;
	public String UpdTime;
	public String Subtitte = null;
	public String Contactphone = null;
	public int Type = 0;
	public String TypeString;
	public String StatusString;
	public String NewPrice = null;
	public String Owner = null;
	public int CategoryId = 0;
	public String Detail = null;
	public String Img = null;
	public String Img1 = null;
	public String Img2 = null;
	public String Img3 = null;
	public String Img4 = null;
	public String SericeInstruction = null;
	public String Ad1 = null;
	public String Ad2 = null;
	public String Ad3 = null;
	public String ParameterValue = null;

	@Override
	public String toString() {
		return "ProductEntity [Id=" + Id + ", BussnessId=" + BussnessId + ", BussnessName=" + BussnessName + ", Price="
				+ Price + ", Name=" + Name + ", Status=" + Status + ", MainImg=" + MainImg + ", IsRecommend="
				+ IsRecommend + ", Sort=" + Sort + ", Stock=" + Stock + ", Adduser=" + Adduser + ", Addtime=" + Addtime
				+ ", UpdUser=" + UpdUser + ", UpdTime=" + UpdTime + ", Subtitte=" + Subtitte + ", Contactphone="
				+ Contactphone + ", Type=" + Type + ", TypeString=" + TypeString + ", StatusString=" + StatusString
				+ ", NewPrice=" + NewPrice + ", Owner=" + Owner + ", CategoryId=" + CategoryId + ", Detail=" + Detail
				+ ", Img=" + Img + ", Img1=" + Img1 + ", Img2=" + Img2 + ", Img3=" + Img3 + ", Img4=" + Img4
				+ ", SericeInstruction=" + SericeInstruction + ", Ad1=" + Ad1 + ", Ad2=" + Ad2 + ", Ad3=" + Ad3
				+ ", ParameterValue=" + ParameterValue + "]";
	}

}

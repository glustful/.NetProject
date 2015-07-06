package com.yoopoon.home.data.user;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import org.json.JSONArray;

import com.yoopoon.common.base.DateHelper;
import com.yoopoon.common.base.OnEachRow;
import com.yoopoon.common.base.Tools;
import com.yoopoon.home.data.storage.ConfigDatabase;
import com.yoopoon.home.data.storage.LocalDatabase;
import com.yoopoon.home.data.storage.SqlCmd;

import android.content.ContentValues;
import android.database.Cursor;
import android.util.Log;


public class UserInfo {
	public static LocalDatabase db;

	public static void init() {
		db = ConfigDatabase.instance();

		if (!db.isTableExsit(UserInfoSql.tb_name_user_info)) {
			db.execCmd(UserInfoSql.createTable());
		}
	}

	public static void showAll() {
		ArrayList<ContentValues> rows = db.queryCmd(UserInfoSql.showAll());
		SqlCmd.showRows(rows);
	}

	

	public static UserInfo findByCode(String code) {
		ContentValues row = db.firstRow(UserInfoSql.findByCode(code));
		return UserInfoSql.fromRow(row);
	}

    public static UserInfo findById(Long id) {
        ContentValues row = db.firstRow(UserInfoSql.findById(id));
        return UserInfoSql.fromRow(row);
    }
    
    public static String findByIdForUserCode(Long id){
    	ContentValues row = db.firstRow(UserInfoSql.findById(id));
    	if(row != null){
    	return row.getAsString(UserInfoSql.col_name_code);
    	}return "";    	
    }
    
    public static String findByCodeForUserName(String userCode){
    	ContentValues row = db.firstRow(UserInfoSql.findByCode(userCode));
    	if(row != null){
    	return row.getAsString(UserInfoSql.col_name_name);
    	}return "";
    }

	public static UserInfo findByEmail(String email) {
		ContentValues row = db.firstRow(UserInfoSql.findByEmail(email));
		return UserInfoSql.fromRow(row);
	}


	

	public UserInfo() {
	}

	public UserInfo(long id,
					String code,
					String name,
					String email,
					String cellphone,
					long status,
					String avatar,
					long createTime,
					
					String gender,
					long birthday,
					String qq,
					String phone,
					String addr) {
		this.id = id;
		this.code = code;
		setName(name);
		this.email = email;
		this.cellphone = cellphone;
		this.status = status;
		this.avatar = avatar;
		this.createTime = SqlCmd.dateOfCol(createTime);
		
		this.gender = gender;
		this.birthday = SqlCmd.dateOfCol(birthday);
		this.qq = qq;
		this.phone = phone;
		this.addr = addr;
	}

	public Boolean exist() {
		return db.hasRow(UserInfoSql.exist(this));
	}

	public Boolean save() {
		if (exist()) {
			
			return db.execCmd(UserInfoSql.update(this));
		} else {
			
			return db.execCmd(UserInfoSql.insertInto(this));
		}
	}

	public Boolean delete() {
		
		return db.execCmd(UserInfoSql.delete(this));
	}

    
	public void updateField(String key, String val) {
		if (key.equals("name")) {
			setName(val);
			save();
		} else if (key.equals("gender")) {
			setGender(val);
			save();
		} else if (key.equals("qq")) {
			setQq(val);
			save();
		} else if (key.equals("cellphone")) {
			setCellphone(val);
			save();
		} else if (key.equals("phone")) {
			setPhone(val);
			save();
		} else if (key.equals("addr")) {
			setAddr(val);
			save();
		} else if (key.equals("birthday")) {
			try {
				SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd");
				setBirthday(df.parse(val));
				save();
			} catch (ParseException e) {
				e.printStackTrace();
			}
		} 
	}

	

	public String toString(boolean isFull) {
		if (isFull) {
			return String.format("[%d][%s][%s][%s][%s][%s][%s][%d][%s][%s][%s][%s][%s][%s][%s][%s]",
					id,
					code,
					name,
					nameFPY,
					namePY,
					email,
					cellphone,
					status,
					avatar,
					createTime.toString(),
					
					gender,
					birthday.toString(),
					qq,
					phone,
					addr);
		} else {
			return String.format("[%d][%s][%s][%s][%s]",
					id,
					code,
					name,
					avatar,
					gender);
		}
	}

	private long id;
	private String code;
	private String name;
	private String nameFPY;
	private String namePY;
	private String email;
	private String cellphone;
	private long status;
	private String avatar;
	private Date createTime;
	private String gender;
	private Date birthday;
	private String qq;
	private String phone;
	private String addr;

	public long getId() {
		return id;
	}

	public void setId(long id) {
		this.id = id;
	}

	public String getCode() {
		return code;
	}

	public void setCode(String code) {
		this.code = code;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
		
	
	}

	public void setNames(String name, String nameFPY, String namePY) {
		this.name = name;
		this.nameFPY = nameFPY;
		this.namePY = namePY;
	}

	public String getNameFPY() {
		return nameFPY;
	}

	public String getNamePY() {
		return namePY;
	}

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getCellphone() {
		return cellphone;
	}

	public void setCellphone(String cellphone) {
		this.cellphone = cellphone;
	}

	public long getStatus() {
		return status;
	}

	public void setStatus(long status) {
		this.status = status;
	}

	public String getAvatar() {
		return avatar;
	}

	public void setAvatar(String avatar) {
		this.avatar = avatar;
	}

	public Date getCreateTime() {
		return createTime;
	}

	public void setCreateTime(Date createTime) {
		this.createTime = createTime;
	}


	public String getGender() {
		return gender;
	}

	public void setGender(String gender) {
		this.gender = gender;
	}

	public Date getBirthday() {
		return birthday;
	}

	public void setBirthday(Date birthday) {
		this.birthday = birthday;
	}

	public String getQq() {
		return qq;
	}

	public void setQq(String qq) {
		this.qq = qq;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public String getAddr() {
		return addr;
	}

	public void setAddr(String addr) {
		this.addr = addr;
	}
	
	@Override
	public String toString(){
		return this.name + "="+this.email;
	}
}

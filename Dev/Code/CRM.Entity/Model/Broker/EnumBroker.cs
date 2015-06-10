/// <summary>
/// 用户类型枚举 （经纪人 broker  ，  管理员 manager）
/// </summary>
public enum EnumUserType
{
    // 经纪人 broker  ，  
    //管理员 admin   ,   
    //商家 merchant    ，   
    //财务 accountant   ，   
    //场秘 secretary    ,   
    //带客人员 waiter
    经纪人 = 0,
    管理员 = 1,
    商家 = 2,
    财务 = 3,
    场秘 = 4,
    带客人员=5,
    普通用户=6
}


/// <summary>
/// 用户状态 （删除0 注销-1 正常1）
/// </summary>
public enum EnumUserState
{
    Delete = 0,
    Cancel = -1,
    OK = 1
}
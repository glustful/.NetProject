/// <summary>
/// �û�����ö�� �������� broker  ��  ����Ա manager��
/// </summary>
public enum EnumUserType
{
    // ������ broker  ��  
    //����Ա admin   ,   
    //�̼� merchant    ��   
    //���� accountant   ��   
    //���� secretary    ,   
    //������Ա waiter
    ������ = 0,
    ����Ա = 1,
    �̼� = 2,
    ���� = 3,
    ���� = 4,
    ������Ա=5,
    ��ͨ�û�=6
}


/// <summary>
/// �û�״̬ ��ɾ��0 ע��-1 ����1��
/// </summary>
public enum EnumUserState
{
    Delete = 0,
    Cancel = -1,
    OK = 1
}
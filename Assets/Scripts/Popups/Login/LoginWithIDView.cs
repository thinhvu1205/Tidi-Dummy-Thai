using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginWithIDView : BaseView
{
    // Start is called before the first frame update
    public static LoginWithIDView instance = null;
    [SerializeField]
    TMP_InputField edbUserName;

    [SerializeField]
    TMP_InputField edbPassword;
    protected override void Awake()
    {
        base.Awake();
        LoginWithIDView.instance = this;
    }
    // Update is called once per frame
   
    public void onClickLogin()
    {
        var strAcc = edbUserName.text;
        var strPass = edbPassword.text;
        Globals.Config.username_normal = strAcc;
        Globals.Config.password_normal = strPass;

        //if (strAcc.Equals(""))
        //{
        //    strAcc = "hienndm";
        //    strPass = "123456hh";
        //}

        if (strAcc.Equals("") | strPass.Equals(""))
        {
            return;
        }

        UIManager.instance.showWatting();
        SocketSend.sendLogin(strAcc, strPass, false);
    }


    public  void onClickClose()
    {
        this.hide(false);
    }
}

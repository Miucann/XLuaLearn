using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using XLua.LuaDLL;

//C#获取lua函数的方式

public class GetLuaFunction : MonoBehaviour
{
    public TextAsset luaSprit;
    LuaEnv luaEnv;
    // Start is called before the first frame update

    void Start()
    {
        luaEnv = new LuaEnv();
        luaEnv.DoString(luaSprit.text);
        //第一种，官方推荐，映射到delegate
        /*             注意事项
         * 1.对于lua function的每一个参数就声明一个输入类型的参数（不是out，ref的）
         * 2.关于多值返回的处理，除第一个返回值外，其他返回值从左往右映射到C#的输出参数（out参数和ref参数）
         * 关于参数的类型，支持各种复杂类型，out，ref修饰的，甚至可以返回另一个delegate
         */

        //获取lua无参函数
        Action func = luaEnv.Global.Get<Action>("Log");
        Add addfunc = luaEnv.Global.Get<Add>("Add");
        MultipleReturn multipeReturn = luaEnv.Global.Get<MultipleReturn>("MultipleReturn");
        func();
        int reslut= addfunc(6, 3);
        Debug.Log("addfunc的返回值：" + reslut);
        string str;
        int firstReturnValue= multipeReturn("I am C#",out str);
        Debug.Log("multipeReturn的两个返回值：" + firstReturnValue + "和" + str);

        //第二种使用LuaFunction 不推荐使用效率差，使用过程都可以明显看到拆装箱
        LuaFunction NoparamFunc = luaEnv.Global.Get<LuaFunction>("Log");
        NoparamFunc.Call();
        LuaFunction AddFunc = luaEnv.Global.Get<LuaFunction>("Add");
        object[] luafunc_reslut= AddFunc.Call(3, 4);
        Debug.Log("用luafunction调用Add返回值：" + luafunc_reslut[0]);
        LuaFunction luafunc_multipeReturn = luaEnv.Global.Get<LuaFunction>("MultipleReturn");
        object[] luafuc_multi_reslut = luafunc_multipeReturn.Call("i am LuaFunction");
        foreach (var item in luafuc_multi_reslut)
        {
            Debug.Log(item);
        }

        //官方推荐如果lua函数不是多返回值函数，建议使用Action和Func，这两个是无CG api，也可以自己去Luafunction里面去加
        Func<int, int, int> CSFunc = luaEnv.Global.Get<Func<int, int, int>>("Add");
        int funcReslut;
        funcReslut= CSFunc(21, 22);
        Debug.Log("使用C# Func来接收加法结果：" + funcReslut);
    }
    //定义三种委托，分别对应lua的无参函数，有参函数，多返回值函数，其中无参函数直接用Action
    //坑点！！！！！！一定要声明为public，然后clear，generate。如果不声明为public会一直跟你报错说
    //你没有使用CSharpCallLua标签
    [CSharpCallLua]
    public delegate int Add(int num1, int num2);
    [CSharpCallLua]
    public delegate int MultipleReturn(string str, out string returnStr);
    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
}

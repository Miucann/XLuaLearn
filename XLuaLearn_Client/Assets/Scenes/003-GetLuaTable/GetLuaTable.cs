using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;
public class GetLuaTable : MonoBehaviour
{
    public TextAsset luaScript;     //因为重点学的是获取luatable，所以这里使用最简便的方式获取lua测试脚本，具体加载脚本的方式看002
    LuaEnv luaEnv;
    // Start is called before the first frame update
    void Start()
    {
        luaEnv = new LuaEnv();
        luaEnv.DoString(luaScript.text);
        //访问lua表方式一:映射到普通的class或struct
        luaTab _luatable= luaEnv.Global.Get<luaTab>("tab");
        Debug.Log(_luatable.num1 + "  " + _luatable.num2+"  "+_luatable.boolValue+"  "+_luatable.num3+" "+_luatable.str); //lua table中常用类型打印
        _luatable.func(); //lua table 中lua函数的执行

    }
    //定义一个class有对应luatable的字段的public属性，而是有无参构造函数即可
    //这个地方一定要注意此处定义的字段名字一定要和lua脚本相关表中的字段名字一模一样，不然会报错
    //可以相对lua表少字段或者多字段，但是名字一定要和lua表中字段名字相同，然就使用不了
    public class luaTab
    {
        public int num1;
        public int num2;
        public bool boolValue;
        public float num3;
        public string str;
        public Action func;
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;               ////第一步导入XLua命名空间
public class HelloCSharp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaEnv luaEnv = new LuaEnv();           //第二步创建Lua虚拟机
        luaEnv.DoString("print('hello C#')");    //DoString函数，里面写lua脚本，具体参见官方API说明
        luaEnv.Dispose();                       //lua虚拟机的释放
    }
}

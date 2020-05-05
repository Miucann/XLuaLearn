using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
public class LuaGetCSharp : MonoBehaviour
{
    LuaEnv luaEnv;
    public TextAsset luaScript;
    // Start is called before the first frame update
    void Start()
    {
        luaEnv = new LuaEnv();
        luaEnv.DoString(luaScript.text); 
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
}

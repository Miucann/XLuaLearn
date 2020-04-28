using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;
public class LoadLua : MonoBehaviour
{
    LuaEnv luaEnv;
    //public TextAsset luaText;           //在Inspector 面板中拖入一个lua文件
    void Start()
    {
        luaEnv = new LuaEnv();
        //第一种方式加载lua脚本
        //在Dostring中直接写lua代码，Dostring 第一个参数是一个string，lua代码就包含在其中，但需要注意书写一定要符合lua的规则
        luaEnv.DoString("print('hello world')");

        //第一种方式变种，因为Dostring中的一个参数本质是一个string类型的所以可以自己存个string然后读取，或者在面板上读取一个TextAsset
        string luaScript = @"print('helle world')";
        luaEnv.DoString(luaScript);
        //luaEnv.DoString(luaText.text);    //加载inspector面板中的lua文件

        //第二种方式，通过在DoString中传入require moudle 的方法，虽然都是在Dostring中执行，但是其加载的方式还是有所差异
        luaEnv.DoString("require 'helloworld'");    //默认的loader是从Resources文件夹中加载，所以我们需要在Resource文件夹中放入相应的lua脚本。
                                                    //后面会将自定义Loader,注意因为Resources下能识别的后缀有限,所以要在lua文件后加.txt

        //第二种方式变种，自定义Loader去加载，默认的loader会从Resources文件夹下进行加载，但是很多时候我们希望从我们制定的文件下去加载lua程序
        //同时在实际项目中，可能会涉及到加密lua文件，然后解密之后再执行，还有从服务器下载lua文件等，这时候都需要用到自定义loader，xlua提供了
        //AddLoader()方法来加入自定义的loader
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require 'helloworldFormLuaFile.lua'");

    }

    //自定义loader,AddLoader中的参数是XLua的CustomLoader，原型是：public delegate byte[] CustomLoader(ref string filepath); 我们根据原型定义好对应委托即可
    //在Scenes 002文件夹下创建一个lua文件夹，用于存放lua脚本，这个只是举例具体的请自己根据需求来
    private byte[] MyLoader(ref string filepath)
    {
        //需要注意的是因为是自定义loader，并且最后会装换成byte[] 数组，所以lua文件的后缀可以直接定义成.txt,不用.lua.txt也可以，但是
        //在使用默认Loader，从Resources下加载的时候，格式必须是.lua.txt,可以自行实验。
        string luaPath = Application.dataPath + "/Scenes/002-LoadLua/lua/" + filepath+".txt";
        string luaSrpit= File.ReadAllText(luaPath);
        byte[] luaBytes = System.Text.Encoding.UTF8.GetBytes(luaSrpit);
        return luaBytes;
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
}

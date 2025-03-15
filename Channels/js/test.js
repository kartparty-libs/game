//create file Assets/Plugins/Web.jslib
var args = [];
//createUnityInstance then save instance
var unity;
//unity push arg
function UnityPushArg(str) {
    args.push(str);
    console.log("push arg " + str);
}
//unity call js
function UnityCall() {
    console.log(args[0]);
    args.length = 0;
    document.callLogin().then(() => {
        console.log("------------- then");
    });
    setTimeout(() => {
        console.log("call unity");
        //call GameConfig Test function
        unity.SendMessage('Entry', 'Test', "hello unity");
    }, 1);
}
//js call unity
function CallUnity(name, args) {
    if (unity == undefined) {
        return;
    }
    //call to JavascriptBridge.cs
    unity.SendMessage('Entry', "JsCall", name, args);
}
var config = config || {};
config.conf = {
    baseUrl: 'https://localhost:12018',
    projectPageSize: '20',
    //cnum: lenovo.util.getParam('cnum'),
    //go: '',
    //url: {
    //    'index': 'apps/feature/',
    //    'app': 'apps/nogame/feature/',
    //    'appHot': 'apps/nogame/hot/',
    //    'appFast': 'apps/nogame/rise/',
    //    'appSort': 'classes/nogame',
    //    'game': 'apps/game/feature/',
    //    'gameHot': 'apps/game/hot/',
    //    'gameFast': 'apps/game/rise/',
    //    'gameSort': 'classes/game',
    //    'sort_2': 'apps/',
    //    'sort_0': 'apps/',
    //    'zone': 'zones',
    //    'search': 'search/',
    //    'bestRecom': 'recommends/feature/',
    //    'relatedRecom': 'recommends/angelapp/com.lenovo.leos.appstore/220/'
    //},
    //liCount: 20,
    //commentCount: 10
};

var constDefine =
{
    tokenKey: "token",
    prjIdKey: "prjID",
    prjModuleIdKey: "prjModuleId",
    paraConfigSelectedIdKey: "paraConfigSelectedId",
    paraGroupIdKey: "paraGroupId",
    defaultGroupNotModifyErrorCode: -2,
};

function pageLoginValidate() {
    var validToken = sessionStorage.getItem(constDefine.tokenKey);
    if (validToken == null || validToken == "") {
        window.location.href = "/htmls/login.html";
        return;
    }
}
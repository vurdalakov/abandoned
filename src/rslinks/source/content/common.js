// RS Links Firefox add-on
// Copyright (c) 2010 Vurdalakov
// http://code.google.com/p/rslinks/
// email: vurdalakov@gmail.com

function rslinks_dump(text)
{
    window.dump("RSLINKS: " + text + "\n");
}

function rslinks_dump2(text)
{
    rslinks_dump("    " + text);
}

// https://developer.mozilla.org/en/JavaScript/Reference/Global_Objects/Error
function RsLinksError(message)
{  
    this.name = "RsLinksError";  
    this.message = message;
}  
RsLinksError.prototype = new Error();  
RsLinksError.prototype.constructor = RsLinksError;  

function rslinks_showException(ex)
{
    rslinks_dump("<EXCEPTION>");
    
    rslinks_dump2('name: ' + ex.name);
    rslinks_dump2('message: ' + ex.message);
    rslinks_dump2('fileName: ' + ex.fileName);
    if (ex.lineNumber != null)
    {
        rslinks_dump2('lineNumber: ' + ex.lineNumber.toString());
    }

    rslinks_dump("</EXCEPTION>");
}

function rslinks_openPage(url)
{
    rslinks_dump("= rslinks_openPage");
    rslinks_dump2(url);

    try
    {
        var browser = null;
        
        if ("undefined" != typeof gBrowser)
        {
            browser = gBrowser;
        }
        else if ((window.opener) && (window.opener.gBrowser))
        {
            browser = window.opener.gBrowser;
        }
        else
        {
            var windowMediator = Components.classes["@mozilla.org/appshell/window-mediator;1"].getService(Components.interfaces.nsIWindowMediator);
            browser = windowMediator.getMostRecentWindow("navigator:browser").gBrowser;
        }

        browser.selectedTab = browser.addTab(url);
    }
    catch (ex)
    {
        rslinks_showException(ex);
        
        window.open(url, "_blank", null);
    }
}

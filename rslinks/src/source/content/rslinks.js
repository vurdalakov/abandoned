// RS Links Firefox add-on
// Copyright (c) 2010 Vurdalakov
// http://code.google.com/p/rslinks/
// email: vurdalakov@gmail.com

var RsLinksListener =
{
    // https://developer.mozilla.org/en/nsIWebProgressListener

    QueryInterface: function(iid)
    {
        if (iid.equals(Components.interfaces.nsIWebProgressListener) ||
            iid.equals(Components.interfaces.nsISupportsWeakReference) ||
            iid.equals(Components.interfaces.nsISupports))
        {
            return this;
        }
        
        throw Components.results.NS_NOINTERFACE;
    },

    onStateChange: function(webProgress, request, stateFlags, status)
    {
        rslinks_dump("= onStateChange");
        rslinks_dump2("flags  = 0x" + stateFlags.toString(16));
        rslinks_dump2("status = 0x" + status.toString(16));
        
        try
        {
            if (stateFlags & Components.interfaces.nsIWebProgressListener.STATE_START)
            {
                // This fires when the load event is initiated

                rslinks_dump("= STATE_START");
            }
            
            if (stateFlags & Components.interfaces.nsIWebProgressListener.STATE_STOP)
            {
                // This fires when the load finishes
                
                rslinks_dump("= STATE_STOP");
                rslinks_dump2(webProgress.DOMWindow.document.URL);
                
                RsLinks.updateDomWindow(webProgress.DOMWindow, true);
            }
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },

    onLocationChange: function(webProgress, request, uri)
    {
        rslinks_dump("= onLocationChange");
        
        try
        {
            if (uri)
            {
                rslinks_dump2(uri.spec);
            }
            else
            {
                rslinks_dump2("null");
            }
            rslinks_dump2(webProgress.DOMWindow.document.URL);
                
            RsLinks.updateDomWindow(webProgress.DOMWindow, false);
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },

    onProgressChange: function(webProgress, request, curSelf, maxSelf, curTot, maxTot) { },
    onStatusChange: function(webProgress, request, status, message) { },
    onSecurityChange: function(webProgress, request, state) { }
}

var RsLinks = 
{
    m_statusbar: null,
    m_preferences: null,
    m_lastDomWindow: null,

    m_goodLinks: [],
    m_goodLinksIndex: [],

    m_badLinks: [],
    m_badLinksIndex: [],

    m_LinksCache: [],
    m_LinksCacheIndex: [],

    copyLinks: function(event, gContextMenu)
    {
        rslinks_dump("= copyLinks");
        
        try
        {
            if (0 == this.m_goodLinks.length)
            {
                rslinks_dump2("No links");
                return;
            }
        
            var platform = navigator.platform.toLowerCase();
            var endOfLine = (platform.indexOf('win') >= 0) ? "\r\n" : (platform.indexOf('mac') >= 0) ? "\r" : "\n";

            var text = "";
            
            for (var i = 0; i < this.m_goodLinks.length; i++)
            {
                text += this.m_goodLinks[i].url + endOfLine;
            }

            if (text.length > 0)
            {
                var clipboard = Components.classes["@mozilla.org/widget/clipboardhelper;1"].getService(Components.interfaces.nsIClipboardHelper);
                clipboard.copyString(text);
            }

            rslinks_dump2(this.m_goodLinks.length + " links copied");
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },

    downloadLinks: function(event, gContextMenu)
    {
        rslinks_dump("= downloadLinks");
        
        try
        {
            if (0 == this.m_goodLinks.length)
            {
                rslinks_dump2("No links");
                return;
            }
        
            if (!("@maone.net/flashgot-service;1" in Components.classes) || (typeof(gFlashGotService) == "undefined") || !gFlashGotService)
            {
                window.openDialog("chrome://rslinks/content/flashgot.xul", "Missing Components", "chrome,dialog,centerscreen,modal");
                return;
            }

            var links = [];
            
            links.referrer = this.m_lastDomWindow.document.URL;
            links.document = this.m_lastDomWindow.document;
            links.browserWindow = gFlashGotService.getBrowserWindow(this.m_lastDomWindow.document);

            for (var i = 0; i < this.m_goodLinks.length; i++)
            {
                var url = this.m_goodLinks[i].url;

                var link = { href: url, description: url, noRedir: false };

                links.push(link);
            }
            
            if (links.length > 0)
            {
                gFlashGotService.download(links);
            }
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },

    defaultAction: function(event, gContextMenu)
    {
        rslinks_dump("= defaultAction");
        
        try
        {
            switch (this.m_preferences.getIntPref("default"))
            {
                case 0:
                    this.copyLinks(event, gContextMenu);
                    break;
                case 1:
                    this.downloadLinks(event, gContextMenu);
                    break;
            }
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },

    showOptions: function(event, gContextMenu)
    {
        rslinks_dump("= showOptions");
        
        try
        {
            window.openDialog("chrome://rslinks/content/options.xul", "Options", "chrome,titlebar,toolbar,centerscreen,modal");
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },

    showAbout: function(event, gContextMenu)
    {
        rslinks_dump("= showAbout");
        
        try
        {
            window.openDialog("chrome://rslinks/content/about.xul", "About", "chrome,dialog,centerscreen,modal");
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },

    addGoodLink: function(result, doc)
    {
        var index = this.m_goodLinksIndex.indexOf(result.id);
        if (index < 0)
        {
            this.m_goodLinksIndex.push(result.id);
            this.m_goodLinks.push(result);
        }
        
        if (doc)
        {
            var index = doc.rslinks_goodLinksIndex.indexOf(result.id);
            if (index < 0)
            {
                doc.rslinks_goodLinksIndex.push(result.id);
                doc.rslinks_goodLinks.push(result);
            }
        }
    },

    addBadLink: function(result, doc)
    {
        var index = this.m_badLinksIndex.indexOf(result.id);
        if (index < 0)
        {
            this.m_badLinksIndex.push(result.id);
            this.m_badLinks.push(result);
        }
        
        if (doc)
        {
            var index = doc.rslinks_badLinksIndex.indexOf(result.id);
            if (index < 0)
            {
                doc.rslinks_badLinksIndex.push(result.id);
                doc.rslinks_badLinks.push(result);
            }
        }
    },

    updateText: function(node, doc)
    {
        if (null == node)
        {
            return;
        }

        if ((1 == node.nodeType) && (("A" == node.nodeName.toUpperCase()) || ("DEL" == node.nodeName.toUpperCase())))
        {
            return;
        }

        if (3 == node.nodeType)
        {
            if (node.textContent.indexOf("rapidshare.com/files/") > 0)
            {
                var parentNode = node.parentNode;
            
                var pattern = /(http:\/\/[a-z0-9\.\-]*rapidshare.com\/files\/.+?\/[$\-_\.+!*'()%a-z0-9]+[$\-_+!*'()%a-z0-9])/;
                var parts = node.textContent.split(pattern);
                
                for (var i = 0; i < parts.length; i++)
                {
                    var result = RsLinks.checkLink(parts[i]);
                    
                    var newNode;
                
                    if (result.valid)
                    {
                        if (result.available)
                        {
                            this.addGoodLink(result, doc);
                            
                            newNode = doc.createTextNode(parts[i]);
                        }
                        else
                        {
                            this.addBadLink(result, doc);
                            
                            newNode = doc.createElement("del");
                            newNode.appendChild(doc.createTextNode(parts[i]));
                        }
                    }
                    else
                    {
                        newNode = doc.createTextNode(parts[i]);
                    }

                    parentNode.insertBefore(newNode, node);
                }
                
                parentNode.removeChild(node);
                
                parentNode.normalize();
            }
        }
        else
        {
            for (var i = 0; i < node.childNodes.length; i++)
            {
                this.updateText(node.childNodes[i], doc);
            }
        }
    },

    updateDocument: function(doc, force)
    {
        rslinks_dump("= updateDocument");

        try
        {
            if (!force && doc.rslinks_processed)
            {
                rslinks_dump2("Already processed");

                for (var i = 0; i < doc.rslinks_goodLinks.length; i++)
                {
                    this.addGoodLink(doc.rslinks_goodLinks[i], null);
                }

                for (var i = 0; i < doc.rslinks_badLinks.length; i++)
                {
                    this.addBadLink(doc.rslinks_badLinks[i], null);
                }

                return;
            }
            
            doc.rslinks_goodLinks = [];
            doc.rslinks_goodLinksIndex = [];

            doc.rslinks_badLinks = [];
            doc.rslinks_badLinksIndex = [];
        
            rslinks_dump2(doc.URL);
            
            var anchors = doc.getElementsByTagName("a");                    
            if (anchors.length)
            {
                for (var i = 0; i < anchors.length; i++)
                {
                    var result = RsLinks.checkLink(anchors[i].href);
                
                    if (result.valid)
                    {
                        if (result.available)
                        {
                            this.addGoodLink(result, doc);
                        }
                        else
                        {
                            this.addBadLink(result, doc);

                            if (anchors[i].innerHTML.length > 0)
                            {
                                anchors[i].innerHTML = "<del>" + anchors[i].innerHTML + "</del>";
                            }
                        }
                    }
                }
            }
            
            var areas = doc.getElementsByTagName("area");
            if (areas.length)
            {
                for (var i = 0; i < areas.length; i++)
                {
                    var result = RsLinks.checkLink(areas[i].href);
                
                    if (result.valid)
                    {
                        if (result.available)
                        {
                            this.addGoodLink(result, doc);
                        }
                        else
                        {
                            this.addBadLink(result, doc);
                        }
                    }
                }
            }
            
            this.updateText(doc.documentElement, doc);

            doc.rslinks_processed = true;
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },

    updateDomWindow: function(domWindow, force)
    {
        rslinks_dump("= updateDomWindow");
        
        this.m_lastDomWindow = domWindow;
        if (!domWindow)
        {
            rslinks_dump2("Null window");
            return;
        }
        
        this.m_statusbar.label = "";
        
        this.m_goodLinks = [];
        this.m_goodLinksIndex = [];

        this.m_badLinks = [];
        this.m_badLinksIndex = [];

        this.updateDocument(domWindow.document, force);
        
        for (var i = 0; i < domWindow.frames.length; i++)
        {
            this.updateDocument(domWindow.frames[i].document, force);
        }
                
        if ((this.m_goodLinksIndex.length + this.m_badLinksIndex.length) > 0)
        {
            var text = "";
            if (this.m_goodLinksIndex.length > 0)
            {
                text += this.m_goodLinksIndex.length + "+";
            }
            if (this.m_badLinksIndex.length > 0)
            {
                if (text.length > 0)
                {
                    text += " ";
                }
                text += this.m_badLinksIndex.length + "-";
            }
            
            this.m_statusbar.label = text;
            this.m_statusbar.collapsed = false;
            
            rslinks_dump2(text);
        }
        else
        {
            rslinks_dump2("No RS links");

            if (this.m_preferences.getBoolPref("nolinks"))
            {
                this.m_statusbar.collapsed = true;
            }
            else
            {
                this.m_statusbar.label = "0";
                this.m_statusbar.collapsed = false;
            }
        }
    },
    
    checkLink: function(url)
    {
        var result = { url: url, valid: false };
        
        // check if it is RapidShare file url
        
        if ((0 == url.length) || (url.indexOf("rapidshare.com/files/") < 0))
        {
            return result;
        }
    
        rslinks_dump("= checkLink");

        rslinks_dump2(url);
    
        try
        {
            // parse file url
        
            var urlPattern = /rapidshare.com\/files\/(\d+)\/(.+)$/;
            var urlParts = url.split(urlPattern);
            
            // check if it is RapidShare file url
            
            if (urlParts.length != 4)
            {
                rslinks_dump2("urlParts.length=" + urlParts.length);
                return result;
            }
            
            // save file id and name, generate unique id
            
            var fileId = urlParts[1];
            var fileName = urlParts[2];
            
            result.id = "ID" + fileId + "#" + fileName.toLowerCase();
            
            // search for cached replies
            
            var index = this.m_LinksCacheIndex.indexOf(result.id);
            if (index >= 0)
            {
                rslinks_dump2("Reusing");
                return this.m_LinksCache[index];
            }
            
            // if not found ask server
            
            var requestUrl = "http://api.rapidshare.com/cgi-bin/rsapi.cgi?sub=checkfiles&files=" + fileId + "&filenames=" + fileName;
        
            var xmlHttpRequest = new XMLHttpRequest();

            xmlHttpRequest.open("GET", requestUrl, false);
            xmlHttpRequest.send(null);

            if (xmlHttpRequest.status != 200)
            {
                throw new RsLinksError("xmlHttpRequest.status=" + xmlHttpRequest.status);
            }
            
            result.reply = xmlHttpRequest.responseText.trim();

            rslinks_dump2(result.reply);
            
            // TODO: check for error response

            // parse server reply
            
            var parts = result.reply.split(",");

            if (parts.length != 7)
            {
                throw new RsLinksError("parts.length=" + parts.length);
            }
            
            result.status = parseInt(parts[4]);
            
            if (isNaN(result.status))
            {
                throw new RsLinksError("isNaN: '" + parts[4] + "'");
            }
            
            result.available = (1 == result.status) || (result.status >= 50);
            result.valid = true;
            
            // cache server reply
            
            this.m_LinksCacheIndex.push(result.id);
            this.m_LinksCache.push(result);
            
            return result;
        }
        catch (ex)
        {
            rslinks_showException(ex);
            return result;
        }
    },

    observe: function(subject, topic, data)
    {
        rslinks_dump("= observe");

        try
        {
            if (topic != "nsPref:changed")
            {
                return;
            }

            switch (data)
            {
                case "nolinks":
                    this.updateDomWindow(this.m_lastDomWindow, false);
                    break;
            }
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },
    
    onLoad: function()
    {
        rslinks_dump("= onLoad");

        try
        {
            this.m_statusbar = document.getElementById("rslinks-statusbar");

            if (this.m_statusbar)
            {
                this.m_preferences = Components.classes["@mozilla.org/preferences-service;1"].getService(Components.interfaces.nsIPrefService).getBranch("rslinks.");

                this.m_preferences.QueryInterface(Components.interfaces.nsIPrefBranch2);
                this.m_preferences.addObserver("", this, false);

                gBrowser.addProgressListener(RsLinksListener);
            }

            rslinks_dump2("Loaded");
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    },

    onUnload: function()
    {
        rslinks_dump("= onUnload");

        try
        {
            if (this.m_statusbar)
            {
                this.m_preferences.removeObserver("", this);
                
                gBrowser.removeProgressListener(RsLinksListener);
            }
            
            rslinks_dump2("Unloaded");
        }
        catch (ex)
        {
            rslinks_showException(ex);
        }
    }
}

window.addEventListener("load", function(e) { RsLinks.onLoad(); }, false);
window.addEventListener("unload", function(e) { RsLinks.onUnload(); }, false);

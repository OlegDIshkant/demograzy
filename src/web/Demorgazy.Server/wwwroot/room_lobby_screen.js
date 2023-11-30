class RoomLobbyScreen
{
    #screen;

    //Labels:
    #startErrorTitle;

    //Buttons:
    #updateBtn;
    #addCandidateBtn;
    #startBtn;

    //List updaters:
    #memberListUpdater;
    #candidateListUpdater;

    //Ids:
    #clientId;
    #roomId;

    //Callbacks:
    #onNeedToAddCandidate;
    #onVotingStarted;
    

    constructor(htmlElement, onNeedToAddCandidate, onVotingStarted)
    {
        this.#defineFields(htmlElement, onNeedToAddCandidate, onVotingStarted);
        this.#hideScreen();
    }


    #defineFields(htmlElement, onNeedToAddCandidate, onVotingStarted)
    {
        this.#screen = htmlElement;

        this.#updateBtn = htmlElement.getElementsByClassName("update_btn")[0];
        this.#addCandidateBtn = htmlElement.getElementsByClassName("add_candidate_btn")[0];
        this.#startBtn = htmlElement.getElementsByClassName("start_btn")[0];

        this.#startErrorTitle = htmlElement.getElementsByClassName("start_error_title")[0];

        this.#onVotingStarted = onVotingStarted;
        this.#onNeedToAddCandidate = onNeedToAddCandidate;

        this.#memberListUpdater = new ListUpdater(htmlElement.getElementsByClassName("members_list")[0]);
        this.#candidateListUpdater = new ListUpdater(htmlElement.getElementsByClassName("candidate_list")[0]);
    }


    #showScreen()
    {
        this.#screen.style.display = "block";
    }


    #hideScreen()
    {
        this.#screen.style.display = "none";
    }


    enable(clientId, roomId)
    {
        this.#clientId = clientId;
        this.#roomId = roomId;
        
        this.#hideStartFailureMsg();
        this.#actualizeListsUpdaters();
        this.#enableButtons();
        this.#showScreen();
        this.#onUpdateBtnClicked();
    }


    #disable()
    {
        this.#disableButtons();
        this.#hideScreen();
    }


    #enableButtons()
    {
        let myself = this;
        this.#updateBtn.onclick = function() { myself.#onUpdateBtnClicked(); };
        this.#addCandidateBtn.onclick = function() { myself.#onAddCandidateButtonClicked(); };
        this.#startBtn.onclick = function() { myself.#onStartBtnClicked(); };

        this.#updateBtn.style.display = "block";
        this.#addCandidateBtn.style.display = "block";
        this.#startBtn.style.display = "block";
    }



    #disableButtons()
    {
        this.#updateBtn.onclick = null;
        this.#addCandidateBtn.onclick = null;
        this.#startBtn.onclick = null;

        this.#updateBtn.style.display = "none";
        this.#addCandidateBtn.style.display = "none";
        this.#startBtn.style.display = "none";
    }


    #actualizeListsUpdaters()
    {
        let url = this.#genUrlToRequestMembers();
        this.#memberListUpdater.setRequestUrl(url);

        url = this.#genUrlToRequestCandidates();
        this.#candidateListUpdater.setRequestUrl(url);
    }



    #onUpdateBtnClicked()
    {
        this.#disableButtons();
        this.#updatePage();
        this.#enableButtons();
    }



    #updatePage()
    {
        this.#memberListUpdater.update();
        this.#candidateListUpdater.update();
    }


    #genUrlToRequestMembers()
    {        
        return new URL('http://localhost:5079/room/' + this.#roomId + '/members');
    }



    #genUrlToRequestCandidates()
    {        
        return new URL('http://localhost:5079/room/' + this.#roomId + '/candidates');
    }



    #onStartBtnClicked()
    {
        this.#disableButtons();

        if (this.#tryStartVoting())
        {
            this.#disable();
            this.#onVotingStarted(this.#clientId, this.#roomId);
        }
        else
        {
            this.#showStartFailureMsg();
            this.#enableButtons();
        }

    }
    

    #tryStartVoting()
    {
        let xhr = new XMLHttpRequest();
        xhr.open('POST', this.#genUrlToStartVoting(), false);
        xhr.send();     

        return xhr.status == 200;
    }


    #showStartFailureMsg()
    {
        this.#startErrorTitle.style.display = "block";
    }


    #hideStartFailureMsg()
    {
        this.#startErrorTitle.style.display = "none";
    }


    #genUrlToStartVoting()
    {
        let url = new URL('http://localhost:5079/room/' + this.#roomId + "/start_voting");
        return url;
    }

    #onAddCandidateButtonClicked()
    {
        this.#disable();
        this.#onNeedToAddCandidate(this.#clientId, this.#roomId);
    }

}


















class ListUpdater
{
    #list;
    #url;

    constructor(htmlList)
    {
        this.#list = htmlList;
    }


    setRequestUrl(url)
    {
        this.#url = url;
    }


    update()
    {
        this.#updateHtmlList(this.#requestItems());
    }


    #requestItems()
    {
        if (this.#url == null)
        {
            throw "no url";
        }

        let xhr = new XMLHttpRequest();
        xhr.open('GET', this.#url, false);
        xhr.send();     

        if (xhr.status == 200)
        {
            return JSON.parse(xhr.response);
        }       

        throw "failed to extract members";
    }



    #updateHtmlList(members)
    {
        this.#clearHtmlList();
        this.#insertIntoHtmlList(members);
    }



    #clearHtmlList()
    {
        var list = this.#list;

        while(list.firstChild)
        {
            list.removeChild(list.firstChild);
        }
    }



    #insertIntoHtmlList(items)
    {
        items.forEach(i => {
            var item = document.createElement("li");
            item.appendChild(document.createTextNode(i));
            this.#list.appendChild(item);
        });
        
    }
}
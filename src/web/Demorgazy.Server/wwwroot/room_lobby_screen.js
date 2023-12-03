class RoomLobbyScreen
{
    #screen;

    //Labels:
    #startErrorTitle;
    #roomTitleLabel;
    #roomIdLabel;

    //Buttons:
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

    #isAdmin;
    

    constructor(htmlElement, onNeedToAddCandidate, onVotingStarted)
    {
        this.#defineFields(htmlElement, onNeedToAddCandidate, onVotingStarted);
        this.#hideScreen();
    }


    #defineFields(htmlElement, onNeedToAddCandidate, onVotingStarted)
    {
        this.#screen = htmlElement;

        this.#addCandidateBtn = htmlElement.getElementsByClassName("add_candidate_btn")[0];
        this.#startBtn = htmlElement.getElementsByClassName("start_btn")[0];

        this.#startErrorTitle = htmlElement.getElementsByClassName("start_error_title")[0];
        this.#roomIdLabel = htmlElement.getElementsByClassName("room_title")[0];
        this.#roomTitleLabel = htmlElement.getElementsByClassName("vote_topic")[0];

        this.#onVotingStarted = onVotingStarted;
        this.#onNeedToAddCandidate = onNeedToAddCandidate;

        this.#memberListUpdater = new ListUpdater(
            htmlElement.getElementsByClassName("members_list")[0],
            (id) => Requests.getClientName(id));
        this.#candidateListUpdater = new ListUpdater(
            htmlElement.getElementsByClassName("candidate_list")[0],
            (id) => Requests.getCandidateName(id));
    }


    #showScreen()
    {
        this.#screen.style.display = "block";
    }


    #hideScreen()
    {
        this.#screen.style.display = "none";
    }


    enable(clientId, roomId, isAdmin)
    {
        this.#clientId = clientId;
        this.#roomId = roomId;
        this.#isAdmin = isAdmin;
        
        this.#setUpRoomTitle();
        this.#hideStartFailureMsg();
        this.#actualizeListsUpdaters();
        this.#enableButtons();
        this.#showScreen();
        this.#startContinuousUpdate();
    }


    #disable()
    {
        this.#disableButtons();
        this.#hideScreen();
    }


    #enableButtons()
    {
        let myself = this;

        if (this.#isAdmin)
        {
            this.#addCandidateBtn.onclick = function() { myself.#onAddCandidateButtonClicked(); };
            this.#addCandidateBtn.style.display = "block";
        
            this.#startBtn.onclick = function() { myself.#onStartBtnClicked(); };
            this.#startBtn.style.display = "block";
        }
    }



    #disableButtons()
    {
        this.#addCandidateBtn.onclick = null;
        this.#startBtn.onclick = null;

        this.#addCandidateBtn.style.display = "none";
        this.#startBtn.style.display = "none";
    }


    #setUpRoomTitle()
    {
        this.#roomIdLabel.innerHTML = "#" + this.#roomId;
        this.#roomTitleLabel.innerHTML = "\"" + Requests.getRoomTitle(this.#roomId) + "\"";
    }


    #actualizeListsUpdaters()
    {
        let url = this.#genUrlToRequestMembers();
        this.#memberListUpdater.setRequestUrl(url);

        url = this.#genUrlToRequestCandidates();
        this.#candidateListUpdater.setRequestUrl(url);
    }



    #startContinuousUpdate()
    {
        new Promise(
            async (resolve) =>
            {
                await this.#runContinuousUpdateAsync();
                resolve();
            }
        )
    }



    async #runContinuousUpdateAsync()
    {
        while (this.#screenIsActive())
        {
            let keepGoing = this.#continuousUpdateIteration();
            if (keepGoing)
            {
                await new Promise(resolve => setTimeout(resolve, 3000));
            }
            else
            {
                break;
            }
        }
    }


    #continuousUpdateIteration()
    {
        this.#disableButtons();
        if (this.#shouldGoToVoteScreenNow())
        {
            this.#goToVoteScreen();
            return false;
        }
        this.#updatePage();
        this.#enableButtons();
        return true;
    }


    #screenIsActive()
    {
        return this.#screen.style.display != 'none';
    }



    #shouldGoToVoteScreenNow()
    {
        return !this.#isAdmin && Requests.isVotingStarted(this.#roomId);
    }



    #goToVoteScreen()
    {
        this.#disable();
        this.#onVotingStarted(this.#clientId, this.#roomId);
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
            this.#goToVoteScreen();
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
    #queryItemLabel;

    constructor(htmlList, queryItemLabel)
    {
        this.#list = htmlList;
        this.#queryItemLabel = queryItemLabel;
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

        try
        {
            xhr.send();     
            if (xhr.status == 200)
            {
                return JSON.parse(xhr.response);
            }       
        }
        catch (e) { }

        return [];
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
            let itemLabel = this.#queryItemLabel(i);
            var item = document.createElement("li");
            item.setAttribute("class", "list-group-item");
            item.appendChild(document.createTextNode(itemLabel));
            this.#list.appendChild(item);
        });
        
    }
}
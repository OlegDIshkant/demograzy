class VoteScreen
{
    #screen;
    #votePanel;
    #onWinnerKnown;
    #clientId;
    #roomId;


    constructor(htmlElement, onWinnerKnown)
    {
        this.#defineFields(htmlElement, onWinnerKnown);
        this.#hideScreen();
    }


    #defineFields(htmlElement, onWinnerKnown)
    {
        this.#screen = htmlElement;
        this.#votePanel = new VotePanel(htmlElement.getElementsByClassName("vote_panel")[0]);
        this.#onWinnerKnown = onWinnerKnown;
    }


    enable(clientId, roomId)
    {
        this.#clientId = clientId;
        this.#roomId = roomId;

        this.#showScreen();
        this.#executeVotingAsync();
    }


    #disable()
    {
        this.#hideScreen();
    }


    #showScreen()
    {
        this.#screen.style.display = 'block';
    }


    #hideScreen()
    {
        this.#screen.style.display = 'none';
    }


    async #executeVotingAsync()
    {

        let winner = this.#getTournamentWinner();
        while(winner == null)
        {
            var activeVerses = this.#getActiveVerses();

            for(let i = 0; i < activeVerses.length; i++)
            {
                let versusId = activeVerses[i];
                await this.#voteInVersusAsync(versusId);
                await this.#waitABit();
            }

            winner = this.#getTournamentWinner();
        }

        this.#disable();
        this.#onWinnerKnown(winner);
    }


    async #waitABit()
    {
        await new Promise((resolve) => setTimeout(resolve, 500));
    }


    #getTournamentWinner(roomId)
    {
        return Requests.getTournamentWinner(this.#roomId);    
    }


    #getActiveVerses()
    {
        return Requests.getActiveVerses(this.#clientId, this.#roomId);
    }



    async #voteInVersusAsync(versusId)
    {
        await this.#votePanel.runAsync(this.#clientId, versusId);
    }
   

}




class VotePanel
{
    #panel;
    #optionLabel1;
    #optionLabel2;
    #voteBtn1;
    #voteBtn2;

    #isBusy = false;

    constructor(htmlElement)
    {
        this.#defineFields(htmlElement);
        this.#hidePanel();
    }


    #defineFields(htmlElement)
    {
        this.#panel = htmlElement;
        this.#optionLabel1 = htmlElement.getElementsByClassName("option_1")[0];
        this.#optionLabel2 = htmlElement.getElementsByClassName("option_2")[0];
        this.#voteBtn1 = htmlElement.getElementsByClassName("choose_1")[0];
        this.#voteBtn2 = htmlElement.getElementsByClassName("choose_2")[0];
    }


    #showPanel()
    {
        this.#panel.style.display = 'block';
    }


    #hidePanel()
    {
        this.#panel.style.display = 'none';
    }


    async runAsync(clientId, versusId)
    {
        if (this.#isBusy)
        {
            throw "Is busy";
        }
        this.#isBusy = true;

        await this.#doActualWork(clientId, versusId);

        this.#isBusy = false;
    }


    async #doActualWork(clientId, versusId)
    {
        this.#hideFailedMsg();
        this.#updateOptionLabels(versusId);
        this.#showPanel();
        await this.#letUserVoteViaGui(clientId, versusId);
        this.#hidePanel();
    }


    async #letUserVoteViaGui(clientId, versusId)
    {
        let myself = this;
        await new Promise(
            (resolve) => myself.#activateButtons(clientId, versusId, resolve)
        );
    }


    #updateOptionLabels(versusId)
    {
        var candidateNames = this.#getCandidateNames(versusId);
        this.#optionLabel1.innerHTML = "\"" + candidateNames[0] + "\"";
        this.#optionLabel2.innerHTML = "\"" + candidateNames[1] + "\"";
    }

    
    #getCandidateNames(versusId)
    {
        var candidateIds = Requests.getVersusCandidateIds(versusId);
        let result = [];
        candidateIds.forEach(id => { result.push(Requests.getCandidateName(id)); });
        return result;
    }

    
    
    #activateButtons(clientId, versusId, onFinish)
    {
        var myself = this;
        this.#voteBtn1.onclick = function() { myself.#onVoteBtnClicked(clientId, versusId, true, onFinish); };
        this.#voteBtn2.onclick = function() { myself.#onVoteBtnClicked(clientId, versusId, false, onFinish); };
    }
    
    
    #deactivateButtons()
    {
        this.#voteBtn1.onclick = null;
        this.#voteBtn2.onclick = null;
    }


    #onVoteBtnClicked(clientId, versusId, votedForFirst, onFinish)
    {
        this.#deactivateButtons();

        let failed = !this.#tryVote(clientId, versusId, votedForFirst); 
        if (failed)
        {
            this.#showFailedMsg();
        }

        if (onFinish)
        {
            onFinish();
        }
    }


    #tryVote(clientId, versusId, voteForFirst)
    {
        return Requests.vote(clientId, versusId, voteForFirst);
    }


    #showFailedMsg()
    {
        
    }


    #hideFailedMsg()
    {
        
    }

}
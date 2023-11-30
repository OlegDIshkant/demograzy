class ClientMenuScreen
{
    #screen;
    #welcomeTitle;
    #createRoomBtn;
    #onStartVoting;

    constructor(htmlElement, onStartVoting)
    {
        this.#defineFields(htmlElement, onStartVoting);
        this.#hideScreen();
    }


    #defineFields(htmlElement, onStartVoting)
    {
        this.#screen = htmlElement;
        this.#welcomeTitle = htmlElement.getElementsByClassName("welcome_title")[0]; 
        this.#createRoomBtn = htmlElement.getElementsByClassName("start_new_voting_btn")[0];
        this.#onStartVoting = onStartVoting; 
    }



    enable(clientId)
    {        
        this.#showScreen();
        this.#greetUserViaWelcomeTitle(clientId);
        this.#enableButtons(clientId);
    }

    #disable()
    {        
        this.#disableButtons();
        this.#hideScreen();
    }


    #showScreen()
    {
        this.#screen.style.display = "block";  
    }


    #hideScreen()
    {
        this.#screen.style.display = "none";  
    }


    #greetUserViaWelcomeTitle(clientId)
    {
        this.#welcomeTitle.innerHTML = "Welcome user '" + clientId + "'!";
    }


    #enableButtons(clientId)
    {
        let myself = this;
        this.#createRoomBtn.onclick = function() { myself.#onStartVotingBtnClicked(clientId); };   
    }


    #disableButtons()
    {
        this.#createRoomBtn.onclick = null;   
    }


    #onStartVotingBtnClicked(clientId)
    {
        this.#disable();
        this.#onStartVoting(clientId);
    }

}
class ClientMenuScreen
{
    #screen;
    #welcomeTitleId;
    #welcomeTitleName;
    #createRoomBtn;
    #joinRoomBtn;
    #onStartCreatingRoom;
    #onStartJoiningRoom;


    constructor(htmlElement, onStartCreatingRoom, onStartJoiningRoom)
    {
        this.#defineFields(htmlElement, onStartCreatingRoom, onStartJoiningRoom);
        this.#hideScreen();
    }


    #defineFields(htmlElement, onStartCreatingRoom, onStartJoiningRoom)
    {
        this.#screen = htmlElement;
        this.#welcomeTitleId = htmlElement.getElementsByClassName("welcome_title_client_id")[0];
        this.#welcomeTitleName = htmlElement.getElementsByClassName("welcome_title_client_name")[0]; 
        this.#createRoomBtn = htmlElement.getElementsByClassName("start_new_voting_btn")[0];
        this.#joinRoomBtn = htmlElement.getElementsByClassName("join_voting_btn")[0];
        this.#onStartCreatingRoom = onStartCreatingRoom; 
        this.#onStartJoiningRoom = onStartJoiningRoom; 
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
        this.#welcomeTitleName.innerHTML = Requests.getClientName(clientId);
        this.#welcomeTitleId.innerHTML = "#" + clientId + "";
    }


    #enableButtons(clientId)
    {
        let myself = this;
        this.#createRoomBtn.onclick = function() { myself.#onStartVotingBtnClicked(clientId); };   
        this.#joinRoomBtn.onclick = function() { myself.#onJoinVotingBtnClicked(clientId); };   
    }


    #disableButtons()
    {
        this.#createRoomBtn.onclick = null;   
        this.#joinRoomBtn.onclick = null;   
    }


    #onStartVotingBtnClicked(clientId)
    {
        this.#disable();
        this.#onStartCreatingRoom(clientId);
    }


    #onJoinVotingBtnClicked(clientId)
    {
        this.#disable();
        this.#onStartJoiningRoom(clientId);
    }

}
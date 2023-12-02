class JoinRoomScreen
{
    #screen;
    #form;
    #joinBtn;
    #errorMsg;
    #roomIdInput;
    #passphraseInput;
    #clientId;
    #onJoinedRoom;


    constructor(htmlElement, onJoinedRoom)
    {
        this.#defineFields(htmlElement, onJoinedRoom);
        this.#hideScreen();
    }


    #defineFields(htmlElement, onJoinedRoom)
    {
        this.#screen = htmlElement;
        this.#form = htmlElement.getElementsByClassName("form")[0];
        this.#form.addEventListener('submit', function(event) { event.preventDefault(); });
        this.#joinBtn = htmlElement.getElementsByClassName("join")[0];
        this.#errorMsg = htmlElement.getElementsByClassName("error_msg")[0];
        this.#roomIdInput = htmlElement.getElementsByClassName("room_id_input")[0];
        this.#passphraseInput = htmlElement.getElementsByClassName("passphrase_input")[0];
        this.#onJoinedRoom = onJoinedRoom;
    }


    enable(clientId)
    {
        this.#clientId = clientId;

        this.#activateButtons();
        this.#hideErrorMsg();
        this.#showScreen();
    }


    #disable()
    {
        this.#deactivateButtons();
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


    #activateButtons()
    {
        let myself = this;
        this.#joinBtn.onclick = function() { myself.#onJoinBtnClicked(); }
        this.#joinBtn.style.display = "block";
    }


    #deactivateButtons()
    {
        this.#joinBtn.onclick = null;
        this.#joinBtn.style.display = "none";
    }


    #onJoinBtnClicked()
    {
        this.#deactivateButtons();

        let roomId = this.#getRoomIdInput();

        if (this.#tryJoinRoom(roomId))
        {
            this.#disable();
            this.#onJoinedRoom(this.#clientId, roomId);
        }
        else
        {
            this.#showErrorMsg();
            this.#activateButtons();
        }
    }


    #tryJoinRoom(roomId)
    {
        return Requests.joinRoom(roomId, this.#clientId, this.#getPassphraseInput());
    }


    #getRoomIdInput()
    {
        return this.#roomIdInput.value;
    }


    #getPassphraseInput()
    {
        return this.#passphraseInput.value;
    }


    #showErrorMsg()
    {
        this.#errorMsg.style.display = "block";
    }


    #hideErrorMsg()
    {
        this.#errorMsg.style.display = "none";
    }

}
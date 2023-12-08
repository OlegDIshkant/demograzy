class CreateRoomScreen
{

    #screen;
    #form;
    #submitBtn;
    #onSuccess;
    #onFail;

    #clientId;



    constructor(htmlElement, onSuccess, onFail)
    {
        this.#defineFields(htmlElement, onSuccess, onFail);
        this.#hideScreen();
    }
    
    
    #defineFields(htmlElement, onSuccess, onFail)
    {
        this.#screen = htmlElement;
        this.#form = htmlElement.getElementsByClassName("form")[0];
        this.#submitBtn = htmlElement.getElementsByClassName("submit")[0];
        this.#onSuccess = onSuccess;
        this.#onFail = onFail;

        this.#form.addEventListener('submit', function(event) { event.preventDefault(); });
    }
    
    
    #hideScreen()
    {
        this.#screen.style.display = "none";
    }
    
    
    #showScreen()
    {
        this.#screen.style.display = "block";
    }


    enable(clientId)
    {
        this.#clientId = clientId;

        this.#enableButtons();
        this.#showScreen();
    }


    #disable()
    {
        this.#disableButtons();
        this.#hideScreen();
    }


    #enableButtons()
    {
        let myself = this;
        this.#submitBtn.onclick = function() { myself.#onSubmitBtnClicked() };
        this.#submitBtn.style.display = "block";
    }


    #disableButtons()
    {
        this.#submitBtn.onclick = null;
        this.#submitBtn.style.display = "none";
    }


    #onSubmitBtnClicked()
    {
        this.#disableButtons();
        this.#requestRoomCreation(this.#clientId, this.#getRoomTitleInput(), this.#getRoomPassphraseInput());
    }


    #getRoomTitleInput()
    {
        return this.#screen.getElementsByClassName("title_input")[0].value;
    }


    #getRoomPassphraseInput()
    {
        return this.#screen.getElementsByClassName("passphrase_input")[0].value;
    }


    #requestRoomCreation(clientId, roomTitle, passphrase)
    {
        let roomId = Requests.createRoom(clientId, roomTitle, passphrase);

        if (roomId != null)
        {
            this.#onRequestCompleted(roomId);
        }
        else
        {
            this.#onRequestFailed();
        }        
    }


    #onRequestCompleted(roomId)
    {
        this.#disable();
        this.#onSuccess(this.#clientId, roomId);
    }


    #onRequestFailed()
    {
        this.#disable();
        this.#onFail(this.#clientId);
    }

}
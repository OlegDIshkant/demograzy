class CreateRoomFailScreen
{
    #screen;
    #backBtn;
    #onSuccess;

    #clientId;

    constructor(htmlElement, onSuccess)
    {
        this.#defineFields(htmlElement, onSuccess);
        this.#hideScreen();
    }


    #defineFields(htmlElement, onSuccess)
    {
        this.#screen = htmlElement;
        this.#backBtn = htmlElement.getElementsByClassName("back_btn")[0];
        this.#onSuccess = onSuccess;
    }


    #showScreen() 
    {
        this.#screen.style.display = "block";
    }


    #hideScreen() 
    {
        this.#screen.style.display = "none";
    }


    enable(clientId)
    {
        this.#clientId = clientId;

        this.#activateButtons();
        this.#showScreen();
    }


    #disable()
    {
        this.#deactivateButtons();
        this.#hideScreen();
    }


    #activateButtons()
    {
        let myself = this;
        this.#backBtn.onclick = function() { myself.#onBackBtnClicked(); }
        this.#backBtn.style.display = "block";
    }


    #deactivateButtons()
    {
        this.#backBtn.onclick = null;
        this.#backBtn.style.display = "none";
    }


    #onBackBtnClicked()
    {
        this.#disable();
        this.#onSuccess(this.#clientId);
    }

}
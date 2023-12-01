class CreateCandidateScreen
{
    #screen;
    #form
    #errorMsg
    #nameInput;
    #roomId;
    #clientId;

    //Buttons:
    #confirmBtn;
    #backBtn;

    //Callbacks:
    #onFinished;



    constructor(htmlElement, onFinished)
    {
        this.#defineFields(htmlElement, onFinished);
        this.#hideScreen();
    }


    #defineFields(htmlElement, onFinished)
    {
        this.#screen = htmlElement;
        this.#form = htmlElement.getElementsByClassName("form")[0];
        this.#confirmBtn = htmlElement.getElementsByClassName("submit")[0];
        this.#backBtn = htmlElement.getElementsByClassName("go_back")[0];
        this.#errorMsg = htmlElement.getElementsByClassName("error_msg")[0];
        this.#nameInput = htmlElement.getElementsByClassName("name_input")[0];
        this.#onFinished = onFinished;
        
        this.#form.addEventListener('submit', function(event) { event.preventDefault(); });
    }


    #showScreen()
    {
        this.#screen.style.display = 'block';
    }


    #hideScreen()
    {
        this.#screen.style.display = 'none';
    }


    enable(clientId, roomId)
    {
        this.#clientId = clientId;
        this.#roomId = roomId;

        this.#enableButtons();
        this.#showScreen();
        this.#hideErrorMsg();
    }


    #disable()
    {
        this.#disableButtons();
        this.#hideScreen();
    }


    #enableButtons()
    {
        let myself = this;
        this.#confirmBtn.onclick = function() { myself.#onConfirmBtnClicked(); };
        this.#backBtn.onclick = function() { myself.#onBackBtnClicked(); }

        this.#confirmBtn.style.display = 'block';
        this.#backBtn.style.display = 'block';
    }


    #disableButtons()
    {
        this.#confirmBtn.onclick = null;
        this.#backBtn.onclick = null;

        this.#confirmBtn.style.display = 'none';
        this.#backBtn.style.display = 'none';
    }


    #onConfirmBtnClicked()
    {
        this.#disableButtons();
        this.#hideErrorMsg();

        if (this.#trySubmitNewCandidate())
        {
            this.#exitThisScreen();
        }
        else
        {
            this.#showErrorMsg();
            this.#enableButtons();
        }
    }


    #exitThisScreen()
    {   
        this.#disable();
        this.#onFinished(this.#clientId, this.#roomId);
    }


    #trySubmitNewCandidate()
    {
        let xhr = new XMLHttpRequest();
        xhr.open('PUT', this.#prepareUrl(), false);
        xhr.send(); 
        return xhr.status == 201;
    }


    #prepareUrl()
    {
        let url = new URL('http://localhost:5079/room/' + this.#roomId +'/candidates/new');
        url.searchParams.set('name', this.#getNameInput());
        return url;
    }


    #getNameInput()
    {
        return this.#nameInput.innerHtml;
    }


    #showErrorMsg()
    {
        this.#errorMsg.style.display = 'block';
    }



    #hideErrorMsg()
    {
        this.#errorMsg.style.display = 'none';
    }



    #onBackBtnClicked()
    {
        this.#exitThisScreen();
    }
    

}
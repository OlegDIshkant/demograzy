
class CreateClientScreen
{
    #wholeScreen;
    #form;
    #clientNameInput;
    #submitBtn;
    #startSuccessScreen;
    #startFailScreen;

    constructor(htmlElement, startSuccessScreen, startFailScreen)
    {
        this.#wholeScreen = htmlElement;
        this.#form = htmlElement.getElementsByClassName("form")[0];
        this.#clientNameInput = htmlElement.getElementsByClassName("client_name_input")[0];
        this.#submitBtn = this.#wholeScreen.getElementsByClassName("submit")[0];
        this.#startSuccessScreen = startSuccessScreen;
        this.#startFailScreen = startFailScreen;
            
        this.#form.addEventListener('submit', function(event) { event.preventDefault(); });
        this.#wholeScreen.style.display = "none";
    }


    
    enable()
    {
        this.#submitBtn.style.display = "block";

        let myself = this;
        this.#submitBtn.onclick = function()
        {
            myself.#submitBtn.style.display = "none";
            myself.#submitClientForm();
        };

        this.#wholeScreen.style.display = "block";
    }



    #disable()
    {
        this.#submitBtn.onclick = null;
        this.#wholeScreen.style.display = "none";
    }


    #submitClientForm()
    {         
        var clientId = Requests.createClient(this.#getClientNameInput());

        if (clientId != null)
        {
            this.#onClientFormSubmitted(clientId);
        }
        else
        {
            this.#onClientFormSubmitFailed();
        }
    }


    #getClientNameInput()
    {
        return this.#clientNameInput.value;
    }


    #onClientFormSubmitted(clientId)
    {
        this.#disable();
        this.#startSuccessScreen(clientId);
    }


    #onClientFormSubmitFailed()
    {         
        this.#disable();
        this.#startFailScreen();
    }
}
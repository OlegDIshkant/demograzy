
class CreateClientScreen
{
    #wholeScreen;
    #form;
    #submitBtn;
    #startSuccessScreen;
    #startFailScreen;
    #currentRequest;

    constructor(htmlElement, startSuccessScreen, startFailScreen)
    {
        this.#wholeScreen = htmlElement;
        this.#form = htmlElement.getElementsByClassName("form")[0];
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
        this.#currentRequest = null;
        this.#submitBtn.onclick = null;
        this.#wholeScreen.style.display = "none";
    }


    #submitClientForm()
    {   
        if (this.#currentRequest != null)
        {
            throw "Some request already exists.";
        }

        let url = new URL('http://localhost:5079/client/new');
        url.searchParams.set('name', 'web_user');

        this.#currentRequest = new XMLHttpRequest();
        this.#currentRequest.open('PUT', url); 

        var myself = this;
        this.#currentRequest.onloadend = function(event) { myself.#onClientFormSubmitted(myself); }
        this.#currentRequest.onerror = function(event) { myself.#onClientFormSubmitFailed(myself); }

        this.#currentRequest.send();            
    }


    #onClientFormSubmitted()
    {         
        if (!this.#currentRequest)
        {
            throw "request is null";
        }

        if (this.#currentRequest.status == 201)
        { 
            var clientId = this.#currentRequest.responseText;
            this.#startSuccessScreen(clientId);
        } 
        else 
        {
            this.#onClientFormSubmitFailed();
        }  
        
        this.#disable();
    }


    #onClientFormSubmitFailed()
    {         
        this.#disable();
        this.#startFailScreen();
    }
}
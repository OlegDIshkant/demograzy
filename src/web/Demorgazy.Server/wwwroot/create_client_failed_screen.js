
class CreateClientFailedScreen
{
    #screen;


    constructor(htmlElement)
    {
        this.#screen = htmlElement;
        this.#screen.style.display = "none";  
    }


    enable()
    {
        this.#screen.style.display = "block";
    }

}
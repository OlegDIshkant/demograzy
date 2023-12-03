class WinnerScreen
{
    #screen;
    #title;


    constructor(htmlElement)
    {
        this.#defineFields(htmlElement);
        this.#hideScreen();
    }


    #defineFields(htmlElement)
    {
        this.#screen = htmlElement;
        this.#title = htmlElement.getElementsByClassName("main_header")[0];
    }


    #showScreen()
    {
        this.#screen.style.display = 'block';
    }


    #hideScreen()
    {
        this.#screen.style.display = 'none';
    }


    enable(winnerId)
    {
        this.#updateHeader(winnerId);
        this.#showScreen();
    }


    #updateHeader(winnerId)
    {
        var winnerName = Requests.getCandidateName(winnerId);
        this.#title.innerHTML = "\"" + winnerName + "\"";
    }




}
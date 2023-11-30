class RoomLobbyScreen
{
    #screen;

    constructor(htmlElement)
    {
        this.#defineFields(htmlElement);
        this.#hideScreen();
    }


    #defineFields(htmlElement)
    {
        this.#screen = htmlElement;
    }


    #showScreen()
    {
        this.#screen.style.display = "block";
    }


    #hideScreen()
    {
        this.#screen.style.display = "none";
    }


    enable(clientId, roomId)
    {
        this.#showScreen();
    }

}
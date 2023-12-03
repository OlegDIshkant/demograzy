class Requests
{
    static getCandidateName(candidateId)
    {
        let request = new XMLHttpRequest();
        let url = this.#genUrlToRequestCandidateName(candidateId);
        request.open('GET', url, false); 
        request.send();
        return request.responseText;   
    }


    static #genUrlToRequestCandidateName(candidateId)
    {
        return new URL('http://localhost:5079/candidate/' + candidateId + '/name');
    }


    static joinRoom(roomId, clientId, passphrase)
    {
        let request = new XMLHttpRequest();
        let url = this.#genUrlToJoinRoom(roomId, clientId, passphrase);
        request.open('PUT', url, false); 
        request.setRequestHeader('passphrase', passphrase);
        request.send();
        return request.status == 200;   
    }


    static #genUrlToJoinRoom(roomId, clientId, passphrase)
    {
        let url = new URL('http://localhost:5079/room/' + roomId + '/members/new');
        url.searchParams.set('memberId', clientId);
        return url;
    }

    static isVotingStarted(roomId)
    {
        let request = new XMLHttpRequest();
        let url = this.#genUrlToCheckIfVotingStarted(roomId);
        request.open('GET', url, false); 
        request.send();
        
        if (request.status == 200)
        {
            return JSON.parse(request.responseText);
        }  
        else
        {
            return false;
        }
    }


    static #genUrlToCheckIfVotingStarted(roomId)
    {
        let url = new URL('http://localhost:5079/room/' + roomId + '/voting_started');
        return url;
    }


    static getClientName(clientId)
    {
        let request = new XMLHttpRequest();
        let url = this.#genUrlToGetClientName(clientId);
        request.open('GET', url, false); 
        request.send();
        
        if (request.status == 200)
        {
            return JSON.parse(request.responseText);
        }  
        else
        {
            return '';
        }
    }


    static #genUrlToGetClientName(clientId)
    {
        let url = new URL('http://localhost:5079/client/' + clientId + '/name');
        return url;
    }


    static getRoomTitle(roomId)
    {
        let request = new XMLHttpRequest();
        let url = this.#genUrlToGetRoomTitle(roomId);
        request.open('GET', url, false); 
        request.send();
        
        if (request.status == 200)
        {
            return JSON.parse(request.responseText);
        }  
        else
        {
            return '';
        }
    }


    static #genUrlToGetRoomTitle(roomId)
    {
        let url = new URL('http://localhost:5079/room/' + roomId + '/title');
        return url;
    }

}
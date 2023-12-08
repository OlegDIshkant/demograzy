class Requests
{
    static #origin;
    static #initialized = false;

    static init(urlOriginForRequests)
    {
        if (this.#initialized)
        {
            throw "Requests already initialized";
        }
        this.#initialized = true;

        this.#origin = urlOriginForRequests;
    }


    static #makeSureThatInitialized()
    {
        if (!this.#initialized)
        {
            throw "Requests not initialized.";
        }
    }


    static getCandidateName(candidateId)
    {
        this.#makeSureThatInitialized();

        let request = new XMLHttpRequest();
        let url = this.#genUrlToRequestCandidateName(candidateId);
        request.open('GET', url, false); 
        request.send();
        return request.responseText;   
    }


    static #genUrlToRequestCandidateName(candidateId)
    {
        return new URL(this.#origin + '/candidate/' + candidateId + '/name');
    }


    static joinRoom(roomId, clientId, passphrase)
    {
        this.#makeSureThatInitialized();

        let request = new XMLHttpRequest();
        let url = this.#genUrlToJoinRoom(roomId, clientId);
        request.open('PUT', url, false); 
        request.setRequestHeader('passphrase', passphrase);
        request.send();
        return request.status == 200;   
    }


    static #genUrlToJoinRoom(roomId, clientId)
    {
        let url = new URL(this.#origin + '/room/' + roomId + '/members/new');
        url.searchParams.set('memberId', clientId);
        return url;
    }

    static isVotingStarted(roomId)
    {
        this.#makeSureThatInitialized();

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
        let url = new URL(this.#origin + '/room/' + roomId + '/voting_started');
        return url;
    }


    static getClientName(clientId)
    {
        this.#makeSureThatInitialized();

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
        let url = new URL(this.#origin + '/client/' + clientId + '/name');
        return url;
    }


    static getRoomTitle(roomId)
    {
        this.#makeSureThatInitialized();

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
        let url = new URL(this.#origin + '/room/' + roomId + '/title');
        return url;
    }


    static createClient(clientName)
    {
        this.#makeSureThatInitialized();

        var request = new XMLHttpRequest();
        request.open('PUT', this.#genUrlToCreateClient(clientName), false); 
        request.send();

        if (request.status == 201)
        {
            return JSON.parse(request.responseText);
        }
        else
        {
            return null;
        }    
    }


    static #genUrlToCreateClient(clientName)
    {
        let url = new URL(this.#origin + '/client/new');
        url.searchParams.set('name', clientName);
        return url;
    }



    static addCandidate(roomId, candidateTitle)
    {
        this.#makeSureThatInitialized();

        let xhr = new XMLHttpRequest();
        xhr.open('PUT', this.#genUrlToAddCandidate(roomId, candidateTitle), false);
        xhr.send(); 
        return xhr.status == 201;
    }




    static #genUrlToAddCandidate(roomId, candidateTitle)
    {
        let url = new URL(this.#origin + '/room/' + roomId +'/candidates/new');
        url.searchParams.set('name', candidateTitle);
        return url;
    }




    static createRoom(ownerClientId, roomTitle, passphrase)
    {
        let xhr = new XMLHttpRequest();
        xhr.open('PUT', this.#genUrlToCreateRoom(ownerClientId, roomTitle), false);
        xhr.setRequestHeader('passphrase', passphrase);
        xhr.send();            

        if (xhr.status == 201)
        {
            return JSON.parse(xhr.responseText);
        }
        else
        {
            return null;
        }
    }



    static #genUrlToCreateRoom(ownerClientId, roomTitle)
    {
        let url = new URL(this.#origin + '/client/' + ownerClientId +'/room/new');
        url.searchParams.set('title', roomTitle);
        return url;
    }


    static getRoomMembers(roomId)
    {
        let xhr = new XMLHttpRequest();
        xhr.open('GET', this.#genUrlToRequestMembers(roomId), false);
        xhr.send();            

        if (xhr.status == 200)
        {
            return JSON.parse(xhr.responseText);
        }
        else
        {
            return null;
        }
    }




    static #genUrlToRequestMembers(roomId)
    {        
        return new URL(this.#origin + '/room/' + roomId + '/members');
    }


    static getCandidates(roomId)
    {
        let xhr = new XMLHttpRequest();
        xhr.open('GET', this.#genUrlToRequestCandidates(roomId), false);
        xhr.send();            

        if (xhr.status == 200)
        {
            return JSON.parse(xhr.responseText);
        }
        else
        {
            return null;
        }
    }



    static #genUrlToRequestCandidates(roomId)
    {        
        return new URL(this.#origin + '/room/' + roomId + '/candidates');
    }



    static startVoting(roomId)
    {
        this.#makeSureThatInitialized();

        let xhr = new XMLHttpRequest();
        xhr.open('POST', this.#genUrlToStartVoting(roomId), false);
        xhr.send();     

        return xhr.status == 200;
    }




    static #genUrlToStartVoting(roomId)
    {
        let url = new URL(this.#origin + '/room/' + roomId + "/start_voting");
        return url;
    }



    static vote(clientId, versusId, voteForFirst)
    {
        let request = new XMLHttpRequest();
        let url = this.#genUrlToVote(clientId, versusId, voteForFirst);
        request.open('POST', url, false); 
        request.send();

        return request.status == 200;    
    }




    static #genUrlToVote(clientId, versusId, voteForFirst)
    {
        let url = new URL(this.#origin + '/versus/' + versusId + '/vote');
        url.searchParams.set('voter', clientId);
        url.searchParams.set('voteForFirst', voteForFirst);
        return url;
    }



    static getVersusCandidateIds(versusId)
    {
        let request = new XMLHttpRequest();
        let url = this.#genUrlToRequestCandidateIds(versusId);
        request.open('GET', url, false); 
        request.send();

        if (request.status == 200)
        {
            return JSON.parse(request.responseText);    
        }
        else
        {
            return [];
        }
    }




    static #genUrlToRequestCandidateIds(versusId)
    {
        return new URL(this.#origin + '/versus/' + versusId + '/candidates');
    }




    static getActiveVerses(clientId, roomId)
    {
        let request = new XMLHttpRequest();
        let url = this.#genUrlToRequestActiveVerses(clientId, roomId);
        request.open('GET', url, false); 
        request.send();
        
        if (request.status == 200)
        {
            return JSON.parse(request.responseText);    
        }
        else
        {
            return [];
        }
    }



    
    static #genUrlToRequestActiveVerses(clientId, roomId)
    {
        return new URL(this.#origin + '/room/' + roomId + '/members/' + clientId + '/active_verses');
    }



    static getTournamentWinner(roomId)
    {
        let request = new XMLHttpRequest();
        let url = this.#genUrlToRequestWinnerId(roomId);
        request.open('GET', url, false); 
        try 
        {
            request.send();
            if (request.status == 200)
            {
                return request.responseText;
            }    
        } 
        catch (error) { }
        
        return null;    
    }



    static #genUrlToRequestWinnerId(roomId)
    {
        return new URL(this.#origin + '/room/' + roomId + '/winner');
    }

}
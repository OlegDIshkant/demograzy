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


}
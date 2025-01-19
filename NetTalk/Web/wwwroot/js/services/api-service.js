export class ApiService {
    constructor(endPoint, authorization) {
        this._endPoint = endPoint;
    }
    
    async _load({
                    url,
                    method = 'GET',
                    body = null,
                    headers = new Headers(),
                }) {

        const response = await fetch(
            `${this._endPoint}/${url}`,
            {method, body, headers},
        );

        try {
            console.log(response)
            ApiService.checkStatus(response);
            return response;
        } catch (err) {
            ApiService.catchError(err);
        }
    }
    static parseResponse(response) {
        return response.json();
    }
    
    static checkStatus(response) {
        if (!response.ok) {
            throw new Error(`${response.status}: ${response.statusText}`);
        }
    }
    
    static catchError(err) {
        throw err;
    }
}
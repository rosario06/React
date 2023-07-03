import axios from "axios";
import env from "react-dotenv"

export default class API{

    static async getCategoria(){
         return axios.get(`${env.API_URL}/rol/Lista`);
         
    }
}


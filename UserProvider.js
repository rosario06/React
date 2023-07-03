import React from "react";
import { useState } from "react";

//el contexto nos permite pasar un valor a lo mas profundo del arbol de componentes
export const userContext = React.createContext();


const UserProvider =({children}) =>{
    //leemos el Item almacenado
    const [user, setUser] = useState(
        window.localStorage.getItem("session_usuario")
    )


    const iniciarSession = (data) =>{
        // almacenamos los datos de la session 
        window.localStorage.setItem("session_usuario",JSON.stringify(data));
        setUser(JSON.stringify(data));
    }

    const cerrarSession =()=>{
        //eliminamos los datos de la session
        window.localStorage.removeItem("session_usuario");
        setUser(null);
    }

    return(
        //usa un Provider para pasar los datos del usario actual al arbol de abajo 
        // Cualquier componente puede leerlo, sin importar cuan profundo sea
        <userContext.Provider value={{user, iniciarSession,cerrarSession}}>
            {children}
        </userContext.Provider>
    )
}


export default UserProvider;
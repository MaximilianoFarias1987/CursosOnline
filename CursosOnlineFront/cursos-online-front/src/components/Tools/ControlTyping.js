import React, {useState, useEffect} from 'react';


export default function ControlTyping(texto, delay){
    const [textoValor, setTextoValor] = useState();

    useEffect(()=>{
        const manejador = setTimeout(() => {
            setTextoValor(texto);
        }, delay);

        return () => {
            clearTimeout(manejador);
        }
    }, [texto]);

    return textoValor;
}

//Lo que hace esta funcion es  que tome el valor del texto una vez que hay pasado un tiempo determinado
//por delay sin que alguien haya escrito nada.
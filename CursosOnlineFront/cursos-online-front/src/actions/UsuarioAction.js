import HttpCliente from '../services/HttpCliente';
import axios from 'axios';

//Agrego esto para que no me pida token al iniciar sesion y al registrar usuario ya que no lo necesito
const instancia = axios.create();
instancia.CancelToken = axios.CancelToken;
instancia.isCancel = axios.isCancel

export const registrarUsuario = usuario =>{
    return new Promise((resolve, eject) =>{
        instancia.post('/Usuario/registrar', usuario).then(response=>{
            resolve(response);
        });
    });
}

//Para que los datos ingresen al reducer necesito invocarlo, por eso le paso como parametro el objeto dispatch
//y luego en el dispatch ingreso lo que quiero que se haga.

export const obtenerUsuarioActual = (dispatch) =>{
    return new Promise((resolve, eject) =>{
        HttpCliente.get('/Usuario').then(response => {

            if (response.data && response.data.imagenPerfil) {
                let fotoPerfil = response.data.imagenPerfil;
                const nuevoFile = 'data:image/' + fotoPerfil.extension + ';base64,' + fotoPerfil.data;
                response.data.imagenPerfil = nuevoFile;
            }

            dispatch({
                type : "INICIAR_SESION",
                sesion : response.data,
                autenticado : true
            });
            resolve(response);
        })
        .catch(error => { resolve(error.response); }); 
    });
}


export const actualizarUsuario = (usuario, dispatch) =>{
    return new Promise((resolve, eject) =>{
        HttpCliente.put('/Usuario/actualizar', usuario)
        .then(response => {
            if (response.data && response.data.imagenPerfil) {
                let fotoPerfil = response.data.imagenPerfil;
                const nuevoFile = 'data:image/' + fotoPerfil.extension + ';base64,' + fotoPerfil.data;
                response.data.imagenPerfil = nuevoFile;
            }

            dispatch({
                type : 'INICIAR_SESION',
                sesion : response.data,
                autenticado : true
            });

            resolve(response);

        })
        .catch(error => {
            resolve(error.response);
        })
    });
};

export const loginUsuario = (usuario, dispatch) =>{
    return new Promise((resolve, eject) => {
        instancia.post('/Usuario/login', usuario).then(response => {

            if (response.data && response.data.imagenPerfil) {
                let fotoPerfil = response.data.imagenPerfil;
                const nuevoFile = 'data:image/' + fotoPerfil.extension + ';base64,' + fotoPerfil.data;
                response.data.imagenPerfil = nuevoFile;
            }
            dispatch({
                type : "INICIAR_SESION",
                sesion : response.data,
                autenticado : true
            })

            resolve(response);
        }).catch(error => {
            resolve(error.response);
        });
    })
}
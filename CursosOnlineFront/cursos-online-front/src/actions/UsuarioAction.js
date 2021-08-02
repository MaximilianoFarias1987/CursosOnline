import HttpCliente from '../services/HttpCliente';

export const registrarUsuario = usuario =>{
    return new Promise((resolve, eject) =>{
        HttpCliente.post('/Usuario/registrar', usuario).then(response=>{
            resolve(response);
        });
    });
}

//Para que los datos ingresen al reducer necesito invocarlo, por eso le paso como parametro el objeto dispatch
//y luego en el dispatch ingreso lo que quiero que se haga.

export const obtenerUsuarioActual = (dispatch) =>{
    return new Promise((resolve, eject) =>{
        HttpCliente.get('/Usuario').then(response => {
            dispatch({
                type : "INICIAR_SESION",
                sesion : response.data,
                autenticado : true
            });
            resolve(response);
        });
    });
}


export const actualizarUsuario = usuario =>{
    return new Promise((resolve, eject) =>{
        HttpCliente.put('/Usuario/actualizar', usuario).then(response => {
            resolve(response);
        }).catch(error => {
            resolve(error.response);
        })
    });
};

export const loginUsuario = usuario =>{
    return new Promise((resolve, eject) => {
        HttpCliente.post('/Usuario/login', usuario).then(response => {
            resolve(response);
        })
    })
}
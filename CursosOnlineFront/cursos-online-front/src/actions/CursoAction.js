import HttpCliente from '../services/HttpCliente';


export const guardarCurso = async (curso, imagen) => {
    const endPointCurso = '/cursos';
    const promesaCurso = HttpCliente.post(endPointCurso, curso);

    if (imagen) {
        const endPointImagenes = '/documentos'; 
        const promesaImagen = HttpCliente.post(endPointImagenes, imagen);
        return await Promise.all([promesaCurso, promesaImagen]);
    }else{
        return await Promise.all([promesaCurso]);
    }
}

//Lo que hago es asignar el valor de los endpoints de cursos y imagen al igual que sus promesas y luego
//en la condicion digo que si imagen existe, entonces que me devuelva todo,
// pero si no existe que e devuelva solo lo de curso

//PAGINACION

export const paginacionCurso = (paginador) => {
    return new Promise((resolve, eject) => {
        HttpCliente.post('/cursos/report', paginador).then(response => {
            resolve(response);
        })
    })
}
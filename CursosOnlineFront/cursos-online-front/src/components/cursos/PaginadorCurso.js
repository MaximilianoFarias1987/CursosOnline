import { Grid, Hidden, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, TextField } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { paginacionCurso } from '../../actions/CursoAction';
import ControlTyping from '../Tools/ControlTyping';

const PaginadorCurso = () => {
    const [paginadorRequest, setPaginadorRequest] = useState({
        titulo : '',
        numeroPagina : 0,
        cantidadElementos: 5
    })

    const [paginadorResponse, setPaginadorResponse] = useState({
        listaRecords : [],
        totalRecords : 0,
        numeroPaginas : 0
    })

    const [textoBusqueda, setTextoBusqueda] = useState('');
    const typingBuscadortexto = ControlTyping(textoBusqueda, 0);

    useEffect(() => {
        
        const obtenerListaCurso = async() => {

            let tituloVariant = '';
            let paginaVariant = paginadorRequest.numeroPagina + 1;
            if(typingBuscadortexto){
                tituloVariant = typingBuscadortexto;
                paginaVariant = 1
            }

            const objetoPaginadorRequest = {
                titulo : tituloVariant,
                numeroPagina : paginaVariant,
                cantidadElementos : paginadorRequest.cantidadElementos
            }

            const response = await paginacionCurso(objetoPaginadorRequest);
            setPaginadorResponse(response.data);
        }

        obtenerListaCurso();

    }, [paginadorRequest, typingBuscadortexto])


    const cambiarPagina = (event, nuevaPagina) =>{
        setPaginadorRequest((anterior) => ({
            ...anterior,
            numeroPagina : parseInt(nuevaPagina)
        }));
    }

    const cambiarCantidadRecords = (event) => {
        setPaginadorRequest((anterior) => ({
            ...anterior,
            cantidadElementos : parseInt(event.target.value),
            numeroPagina : 0
        }));
    }

    console.log('curso', paginadorResponse.listaRecords.titulo);

    return (
        <div style={{padding:'15px', width: '100%'}}>
            <Grid container style={{paddingTop: '20px', paddingBottom:'20px'}}>
                <Grid item xs={12} sm={4} md={6}>
                    <TextField
                        fullWidth
                        name='txtBusquedaCurso'
                        variant='outlined'
                        label='Busca tu curso'
                        onChange={e => setTextoBusqueda(e.target.value)}
                    />
                </Grid>
            </Grid>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell align='left'>Cursos</TableCell>
                            {/* Con el Hidden mdDown hago que en version movil se oculten esas columnas */}
                            <Hidden mdDown> 
                                <TableCell align='left'>Descripcion</TableCell>
                                <TableCell align='left'>Fecha Publicacion</TableCell>
                                <TableCell align='left'>Precio Lista</TableCell>
                                <TableCell align='left'>Precio Promocion</TableCell>
                            </Hidden>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {paginadorResponse.listaRecords.map((curso) => (
                            //  a key={curso.key} si se lo saco funciona igualmente bien
                            <TableRow key={curso.key}>
                                <TableCell align='left'>{curso.Titulo}</TableCell>
                                {/* Con el Hidden mdDown hago que en version movil se oculten esas columnas */}
                                <Hidden mdDown>
                                    <TableCell align='left'>{curso.Descripcion}</TableCell>
                                    <TableCell align='left'>{(new Date(curso.FechaPublicacion)).toLocaleString()}</TableCell>
                                    <TableCell align='left'>{curso.PrecioActual}</TableCell>
                                    <TableCell align='left'>{curso.Promocion}</TableCell>
                                </Hidden>
                                
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <TablePagination
                rowsPerPageOptions={[5, 10, 25]}
                count={paginadorResponse.totalRecords}
                rowsPerPage={paginadorRequest.cantidadElementos}
                page={paginadorRequest.numeroPagina}
                onChangePage = {cambiarPagina}
                onChangeRowsPerPage = {cambiarCantidadRecords}
                labelRowsPerPage = 'Cursos por pagina'
            />
        </div>
    );
};

export default PaginadorCurso;
import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { actualizarUsuario, obtenerUsuarioActual } from '../../actions/UsuarioAction';
import { useStateValue } from '../../contexto/store';
import style from '../Tools/Style';



const PerfilUsuario = () => {

    const [{sesionUsuario}, dispatch] = useStateValue();

    const [usuario, setUsuario] = useState({
        nombreCompleto : '',
        email : '',
        password : '',
        confirmarPassword: '',
        userName : ''
    })
    
    const ingresarValorMemoria = e =>{
        const {name, value} = e.target;
        setUsuario(anterior => ({
            ...anterior,
            [name] : value
        }))
    }

    useEffect(() => {
        obtenerUsuarioActual(dispatch).then(response => {
            console.log('Esta es la data del objeto response del usuario actual', response);
            // console.log('Variable global ', sesionUsuario.usuario.nombreCompleto);
            setUsuario(response.data);
        });
    }, []);

    const actualizar = e => {
        e.preventDefault();
        actualizarUsuario(usuario).then(response => {
            if (response.status === 200) {
                dispatch({
                    type : "OPEN_SNACKBAR",
                    openMensaje : {
                        open : true,
                        mensaje : "Se guardaron exitosamente los cambios en Perfil Usuario"
                    }
                })
                window.localStorage.setItem("token_seguridad", response.data.token);
            }else{
                dispatch({
                    type : "OPEN_SNACKBAR",
                    openMensaje : {
                        open : true,
                        mensaje : "Errores al intentar guardar en : " + Object.keys(response.data.errors)
                    }
                })
            }
        })
    }

    return (
        <Container component='main' maxWidth='md' justify='center'>
            <div style={style.paper}>
                <Typography component='h1' variant='h5'>
                    Perfil de Usuario
                </Typography>
            </div>
            <form style={style.form}>
                <Grid container spacing={2}>
                    <Grid item xs={12} md={12}>
                        <TextField name='nombreCompleto' value={usuario.nombreCompleto || ""} onChange={ingresarValorMemoria} variant='outlined' fullWidth label='Ingrese nombre y apellido'/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name='userName' value={usuario.userName || ""} onChange={ingresarValorMemoria} variant='outlined' fullWidth label='Ingrese UserName'/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name='email' value={usuario.email || ""} onChange={ingresarValorMemoria} variant='outlined' fullWidth label='Ingrese email'/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name='password' value={usuario.password || ""} onChange={ingresarValorMemoria} type='password' variant='outlined' fullWidth label='Ingrese password'/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name='confirmarPassword' value={usuario.confirmarPassword || ""} onChange={ingresarValorMemoria} type='password' variant='outlined' fullWidth label='Confirme password'/>
                    </Grid>
                </Grid>
                <Grid container justifyContent='center'>
                    <Grid item xs={12} md={6}>
                        <Button type='submit' onClick={actualizar} fullWidth variant='contained' size='large' color='primary' style={style.submit}>
                            Guardar Datos
                        </Button>
                    </Grid>
                </Grid>
            </form>
        </Container>
    );
};

export default PerfilUsuario;
import { Avatar, Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { actualizarUsuario, obtenerUsuarioActual } from '../../actions/UsuarioAction';
import { useStateValue } from '../../contexto/store';
import style from '../Tools/Style';
import reactFoto from '../../logo.svg';
import {v4 as uuidv4} from 'uuid';
import ImageUploader from 'react-images-upload';
import { obtenerDataImagen } from '../../actions/ImagenAction';



const PerfilUsuario = () => {

    const [{sesionUsuario}, dispatch] = useStateValue();

    const [usuario, setUsuario] = useState({
        nombreCompleto : '',
        email : '',
        password : '',
        confirmarPassword: '',
        userName : '',
        imagenPerfil : null,
        fotoUrl : ''
    })
    
    const ingresarValorMemoria = e =>{
        const {name, value} = e.target;
        setUsuario(anterior => ({
            ...anterior,
            [name] : value
        }))
    }

    //Con este useEffect cargo los valores del usuario actual que traigo por medio de la variable global
    useEffect(() => {
        setUsuario(sesionUsuario.usuario);
        setUsuario(anterior => ({
            ...anterior,
            fotoUrl : sesionUsuario.usuario.imagenPerfil,
            imagenPerfil : null
        }))
    }, []);

    const actualizar = e => {
        e.preventDefault();
        actualizarUsuario(usuario, dispatch).then(response => {
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

    const subirFoto = imagenes => {
        //Obtengo el primer archivo que recibo en el arrego de imagenes
        const foto = imagenes[0];
        //Lo convierto a una url
        const fotoUrl = URL.createObjectURL(foto);

        obtenerDataImagen(foto).then(respuesta => {
            console.log('respuesta', respuesta);
            setUsuario(anterior => ({
                ...anterior,
                imagenPerfil : respuesta, //Almaceno el archivo en formato file
                fotoUrl : fotoUrl //El archivo en formato url
            }));
        }); 
    }

    const fotoKey = uuidv4()

    return (
        <Container component='main' maxWidth='md' justifyContent='center'>
            <div style={style.paper}>
                <Avatar style={style.avatar} src={usuario.fotoUrl || reactFoto} />
                <Typography component='h1' variant='h5'>
                    Perfil de Usuario
                </Typography>
           
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
                    <Grid item xs={12} md={12}>
                        <ImageUploader
                            withIcon = {false}
                            key = {fotoKey}
                            singleImage = {true}
                            buttonText = 'Seleccione una imagen de perfil'
                            onChange = {subirFoto}
                            imgExtension = {[".jpg", ".gif", ".png", ".jpeg"]}
                            maxFileSize = {5242880}
                        />
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
            </div>
        </Container>
    );
};

export default PerfilUsuario;
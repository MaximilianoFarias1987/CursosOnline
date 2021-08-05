import { Avatar, List, ListItem, ListItemText } from '@material-ui/core';
import React from 'react';
import { Link } from 'react-router-dom';
import FotoUsuarioTemp from "../../../logo.svg";

export const MenuDerecha = ({
    classes,
    usuario,
    salirSesion
}) => (
    <div className={classes.list}>
        <List>
            <ListItem buttom component={Link}>
                <Avatar src={usuario.imagenPerfil || FotoUsuarioTemp}/>
                <ListItemText classes={{primary : classes.listItemText}} primary={usuario ? usuario.nombreCompleto : ''}/>
            </ListItem>
            <ListItem button onClick={salirSesion}>
                <ListItemText classes={{primary : classes.listItemText}} primary='Cerrar Sesión'/>
            </ListItem>
        </List>
    </div>
);
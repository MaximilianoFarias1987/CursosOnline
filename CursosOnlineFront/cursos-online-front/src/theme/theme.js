import { createTheme} from '@material-ui/core/styles';


const theme = createTheme({
    palette : {
        primary : {
            light : "#63a4fff", //Esto hace que el elemento  cambie de color de fondo por ej cuando pasamos el mouse
            main : "#1976d2", //Color principal de la pagina
            dark : "#004ba0", //El color oscuro
            contrastText : "#ecfad8" //color de contraste del texto
        }
        
    },
});

export default theme;
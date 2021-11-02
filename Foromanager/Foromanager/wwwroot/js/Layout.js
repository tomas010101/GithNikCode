function codito ()
{
    document.getElementById("Barra").classList.toggle("Aparecer");
}
function boton ()
{
    document.getElementById("Codito-boton").classList.toggle("Codito-change");
}
document.getElementById("Codito-boton").onclick = function ()
{
    codito();
    boton();
}
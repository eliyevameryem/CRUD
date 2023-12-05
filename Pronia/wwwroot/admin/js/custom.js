const addbasketbtns = document.querySelectorAll(".add-basket-button")

addbasketbtns.forEach(btn => {
    btn.addEventListener("click", function (e) {
        e.preventDefault();


        var endpoint = btn.getAttribute("href")
        console.log(endpoint)

        
    })
})
class TaxCalculatorService{
    serverPort = "8080"
    serverUrl = "http://localhost"
    columns = ["Gross Annual Salary", "Gross Monthly Salary", "Net Annual Salary", "Net Monthly Salary", "Annual Tax Paid", "Monthly Tax Paid"]
    jsonKeys = ["grossAnnualSalary", "grossMonthlySalary", "netAnnualSalary", "netMonthlySalary", "annualTaxPaid", "monthlyTaxPaid"];
    
    async getTaxData(){
        const url = `${this.serverUrl}:${this.serverPort}/api/tax/calculator/v1/calculate`
        const grossSalary = document.getElementById('grossSalary').value

        try{
            const response = await fetch(url, {
                method: "POST",
                body: JSON.stringify({grossAnnualSalary: grossSalary}),
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if(!response.ok){
                throw new Error(`Response status: ${response.status}`)
            }

            const json = await response.json();

            this.createTaxColumns(json)
        }
        catch(error){
            console.error(error.message);
        }
    }

    createTaxColumns(json){
        
        let divContainer = document.getElementById("tax-data-container")

        if(!divContainer){
             divContainer = document.createElement("div")
             divContainer.id = "tax-data-container";
             document.body.appendChild(divContainer);
        }
        else{
            divContainer.innerHTML = ""
        }

        for(let i=0; i< this.columns.length; i++){
            const columnName = document.createTextNode(`${this.columns[i]}: `)
            
            const key = this.jsonKeys[i]
            const columnValue = document.createTextNode(json[key])

            const paragraph = document.createElement("p")
            paragraph.appendChild(columnName)
            paragraph.appendChild(columnValue)

            divContainer.appendChild(paragraph)
        }

        document.body.appendChild(divContainer);
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const taxCalculatorService = new TaxCalculatorService();

    document.getElementById("calculateButton").addEventListener("click", (event) => {
        event.preventDefault();
        taxCalculatorService.getTaxData();
    });
});
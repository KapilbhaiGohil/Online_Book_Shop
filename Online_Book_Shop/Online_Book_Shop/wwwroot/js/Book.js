let timeout = null;
let result = document.getElementById("result");
let selectedAuthors = [];
let availableOptions = [];
document.getElementById("SearchTags").addEventListener("keyup", () => {
    clearTimeout(timeout)
    timeout = setTimeout(async () => {
        await LiveSearch();
    }, 400)
});
async function LiveSearch() {
    let value = document.getElementById("SearchTags").value;
    if (value.length > 2) {
        const res = await fetch("/Admin/SearchTags", {
            body: JSON.stringify({ search: value }),
            method: "POST",
            headers: {
                "Content-type": "application/json",
            },
        });
        if (res.ok) {
            const data = await res.json();
            availableOptions = data
            RefreshOption();
        } else {
            showMsg("Failed To Fetch Data From Server",1);
        }
    } else {
        result.innerHTML = "";
    }
}
let authors = document.getElementById("Authors");
function AddAuthor(id, firstname, lastname) {
    selectedAuthors.push({ UserId: id, firstName: firstname, lastName: lastname })
    document.getElementById("SearchTags").value = "";
    RefreshAuthor();
}
function RemoveAuthor(id) {
    selectedAuthors = selectedAuthors.filter(a => a.UserId != id);
    availableOptions = [];
    RefreshAuthor();
    RefreshOption(false);
}
function RefreshAuthor() {
    let updated = '';
    for (let i = 0; i < selectedAuthors.length; i++) {
        updated += `<div id="selected">
                        <p>${selectedAuthors[i].firstName + ` ` + selectedAuthors[i].lastName}</p>
                        <button type="button" onclick="RemoveAuthor(${selectedAuthors[i].UserId})">Remove</button>
                </div>`;
    }
    authors.innerHTML = updated;
    RefreshOption()
}
function RefreshOption(showMsg=true) {
    console.log(selectedAuthors);
    console.log(availableOptions);
    let innerHtml = "";
    for (let i = 0; i < availableOptions.length; i++) {
        let isOk = selectedAuthors.findIndex(au => au.UserId == availableOptions[i].userId);
        if (isOk == -1) {
            innerHtml += `<p onclick="AddAuthor(${availableOptions[i].userId},'${availableOptions[i].firstName}','${availableOptions[i].lastName}')">${availableOptions[i].firstName + ` ` + availableOptions[i].lastName}</p>`
        }
    }
    console.log("selected authors",selectedAuthors)
    console.log("available options",availableOptions)
    result.innerHTML = innerHtml;
    if (availableOptions.length == 0 && showMsg) {
        result.innerHTML = `<p>No Author Found with Given Name</p>`
    }
}



//optins fetching part
let TypeOptions = [];
let LanguageOptions = [];
let CountryOptions = [];
let Type = document.getElementById("Type");
let Country = document.getElementById("Country");
let Language = document.getElementById("Language");

(async function fetchData() {
    const res = await fetch("/Admin/GetData", {
        method: "POST",
        headers: {
            "Content-Type": "Application/json",
        },
    })
    if (res.ok) {
        const data = await res.json();
        TypeOptions = data.type;
        LanguageOptions = data.languages;
        CountryOptions = data.countries;
        addOptions();
        console.log(TypeOptions, LanguageOptions,CountryOptions)
    } else {
        showMsg("Failed To Fetch Data From Server", 1);
    }
})();
function addOptions() {
 
    let options = `<option value=''>Select Book Type</option>`;
    for (let i = 0; i < TypeOptions.length; i++) {
        options += `<option value = '${TypeOptions[i]}'>${TypeOptions[i]}</option>`
    }
    let lan = `<option value=''>Select Language</option>`;
    for (let i = 0; i < LanguageOptions.length; i++) {
        lan += `<option value = '${LanguageOptions[i]}'>${LanguageOptions[i]}</option>`
    }
    let cou = `<option value=''>Select Country</option>`;
    for (let i = 0; i < CountryOptions.length; i++) {
        cou += `<option value = '${CountryOptions[i]}'>${CountryOptions[i]}</option>`
    }
    console.log(lan)
    console.log(Language.innerHTML)
    Type.innerHTML = options;
    Language.innerHTML = lan;
    Country.innerHTML = cou;
};

//form submission part
let btn = document.getElementById("BookFormBtn");
let form = document.getElementById("BookForm");
btn.addEventListener("click", (e) => {
    if (form.checkValidity()) {
        if (authors.innerHTML == '') {
            ShowMsg("Select The Atleast One Author", 1);
        } else {
            let authorData = document.getElementById("authorData");
            authorData.value = JSON.stringify(selectedAuthors);
            console.log(authorData.value)
            form.submit();
        }
    } else {
        ShowMsg("Pls Fill All The Details", 1);
    }
 
})


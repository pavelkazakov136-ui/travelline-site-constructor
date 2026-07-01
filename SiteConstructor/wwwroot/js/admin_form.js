async function loadform() {
  const res = await fetch('/api/content');
  const data = await res.json();
  renderForm(data.form);
}

function renderForm(form){
    const box = document.getElementById('form');
    box.innerHTML =  `
        <input type="text" value="${form.title}" data-field="title"    placeholder="Заголовок">
        <input type="text" value="${form.subtitle}" data-field="subtitle" placeholder="Подзаголовок">
        <input type="text" value="${form.button}" data-field="button"   placeholder="Текс кнопки">
        
        `
}

function collectForm(form){
    return{
        title: form.querySelector('[data-field="title"]').value,
        subtitle: form.querySelector('[data-field="subtitle"]').value,
        button: form.querySelector('[data-field="button"]').value
    }
}

async function updatedForm(){
    const form = document.getElementById("form")
    const res = await fetch('/api/content/form', {
        method:'PUT',
        headers:{'content-Type':'application/json'},
        body: JSON.stringify(collectForm(form))
    });
    const msg = document.getElementById("form-msg");
    msg.textContent = res.ok ? 'Сохранено ✓' : 'Ошибка при сохранении';

}

document.getElementById("add-form").addEventListener('click', updatedForm);

loadform();


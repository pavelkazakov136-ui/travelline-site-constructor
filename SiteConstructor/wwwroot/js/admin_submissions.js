async function loadSubmissions() {
    const res = await fetch('/api/submissions');   
    if (!res.ok) return;                          
    const submissions = await res.json();
    renderSubmissions(submissions);
}

function renderSubmissions(submissions) {
    const box = document.getElementById('submissions-list');
    if (submissions.length === 0) {
        box.innerHTML = '<p>Заявок пока нет.</p>';
        return;
    }
    box.innerHTML = submissions.map(s => `
        <div class="submission-card" data-id="${s.id}">
            <small>${new Date(s.createdAt).toLocaleString('ru-RU')}</small>
            <p><b>${s.name}</b> — ${s.direction || 'направление не указано'}</p>
            <p>Телефон: ${s.phone}</p>
            <p>Email: ${s.email}</p>
            <p>Резюме: <a href="${s.resume}" target="_blank">${s.resume}</a></p>
            <button type="button" data-action="delete" data-id="${s.id}" class="secondary">Удалить</button>
        </div>`
    ).join('');
}
async function deleteSubmission(id) {
    const res = await fetch(`/api/submissions/${id}`, { method: 'DELETE' });
    if (res.ok) loadSubmissions();   
}
document.getElementById('submissions-list').addEventListener('click', (event) => {
    const action = event.target.dataset.action;
    const id = Number(event.target.dataset.id);
    if (action === 'delete') deleteSubmission(id);
});
loadSubmissions();
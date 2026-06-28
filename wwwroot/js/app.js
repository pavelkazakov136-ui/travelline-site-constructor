async function loadContent() { 
  const res = await fetch('/api/content'); 
  const data = await res.json(); 
  
  renderHero(data.hero)
  renderTeam(data.team)
  renderVacancies(data.vacancies)
  setupStatsScroll() 
}

function renderHero(hero) {
  document.querySelector('.hero__title').innerHTML = hero.subtitle.replaceAll('\n', '<br>');
  const stats__list = document.querySelector('.stats__list');
  let htmlContent = hero.stats.map(stat => {
    const cleanLabel = stat.label.replaceAll('\n', '<br>')
    return `<li class="stats__item" data-value="${stat.value}">${cleanLabel}</li>`}).join('');
  stats__list.innerHTML = htmlContent;
}

function renderStatValue(value) {
  return value.endsWith('.svg')
    ? `<img class="stats__value-logo" src="images/${value}" alt="TravelLine">`
    : value;
}

function setupStatsScroll() {
  const valueEl = document.querySelector('.stats__value');
  const items = document.querySelectorAll('.stats__item');

  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        valueEl.innerHTML = renderStatValue(entry.target.dataset.value);
        items.forEach(i => i.classList.remove('stats__item--active'));
        entry.target.classList.add('stats__item--active');
      }
    });
  }, {
    root: null,                      
    rootMargin: '-50% 0px -50% 0px', 
    threshold: 0
  });

  items.forEach(item => observer.observe(item));
}

function renderTeam(team) {
  team.sort((a, b) => a.id - b.id);
  const team__track = document.querySelector('.team__track')
  let htmlContent = team.map(personality => 
    `<li class="team__card">
          <img class="team__photo" src="${personality.photo}" alt="${personality.name}" />
          <div class="team__meta">
            <span class="team__name">${personality.name}</span>
            <span class="team__role">${personality.position}</span>
          </div>
        </li>`
  ).join('');
  team__track.innerHTML = htmlContent; 
}

function renderVacancies(vacancies){
  vacancies.sort((a, b) => a.id - b.id);
  const vacancies__list = document.querySelector('.vacancies__list');
  let htmlContent = vacancies.map(vacancy => 
    `<li class="vacancies__item">
          <h3 class="vacancies__item-title">${vacancy.title}</h3>
          <div class="vacancies__item-footer">
            <p class="vacancies__item-place">${vacancy.format}</p>
            <img class="vacancies__item-icon" src="images/hhru.svg" alt="hh.ru" />
          </div>
          <a class="vacancies__item-link" href="${vacancy.url}" target="_blank" rel="noopener" aria-label="${vacancy.title}"></a>
        </li>`
  ).join('');
  htmlContent += 
        `<li class="vacancies__item vacancies__item--more">
          <h3 class="vacancies__item-title">Еще больше вакансий на HeadHunter →</h3>
          <a class="vacancies__item-link" href="https://yoshkar-ola.hh.ru/search/vacancy?from=employerPage&amp;hhtmFrom=employer&amp;employer_id=1136961" target="_blank" rel="noopener" aria-label="Все вакансии на HeadHunter"></a>
        </li>`
  vacancies__list.innerHTML = htmlContent;

}

loadContent();   
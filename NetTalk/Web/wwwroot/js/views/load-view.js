import AbstractView from '../framework/view/abstract-view.js';

const createLoadingTemplate = () => `
<div class="d-flex justify-content-center load">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>`;

export default class LoadingView extends AbstractView {
    get template() {
        return createLoadingTemplate();
    }
}
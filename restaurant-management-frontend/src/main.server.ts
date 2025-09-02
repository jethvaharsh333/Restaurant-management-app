import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { config } from './app/app.config.server';

const bootstrap = () => bootstrapApplication(App, config).catch((error) => {
    console.error('SSR Error:', JSON.stringify(error, Object.getOwnPropertyNames(error), 2));
});

export default bootstrap;

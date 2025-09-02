import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

export type ToastType = 'success' | 'error' | 'warning' | 'info' | 'default';

export interface Toast {
  id: string;
  message: string;
  type: ToastType;
  duration?: number; // 0 means no auto-dismiss
  dismissible?: boolean;
  timestamp: number;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toastsSubject = new BehaviorSubject<Toast[]>([]);
  public toasts$ = this.toastsSubject.asObservable();
  private timers = new Map<string, any>();

  constructor() {}

  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  show(message: string, type: ToastType = 'default', duration: number = 3000): string {
    const id = this.generateId();
    const toast: Toast = {
      id,
      message,
      type,
      duration,
      dismissible: true,
      timestamp: Date.now()
    };

    const currentToasts = this.toastsSubject.value;
    this.toastsSubject.next([toast, ...currentToasts]);

    // Auto-dismiss after duration
    if (duration > 0) {
      this.setTimer(id, duration);
    }

    return id;
  }

  success(message: string, duration?: number): string {
    return this.show(message, 'success', duration);
  }

  error(message: string, duration?: number): string {
    return this.show(message, 'error', duration || 4000); // Errors stay longer
  }

  warning(message: string, duration?: number): string {
    return this.show(message, 'warning', duration);
  }

  info(message: string, duration?: number): string {
    return this.show(message, 'info', duration);
  }

  loading(message: string): string {
    return this.show(message, 'info', 0); // Loading toasts don't auto-dismiss
  }

  remove(id: string): void {
    const currentToasts = this.toastsSubject.value;
    this.toastsSubject.next(currentToasts.filter(toast => toast.id !== id));
    this.clearTimer(id);
  }

  removeAll(): void {
    this.toastsSubject.next([]);
    this.clearAllTimers();
  }

  pauseTimer(id: string): void {
    if (this.timers.has(id)) {
      clearTimeout(this.timers.get(id));
    }
  }

  resumeTimer(id: string, remainingTime?: number): void {
    const toast = this.toastsSubject.value.find(t => t.id === id);
    if (toast && toast.duration && toast.duration > 0) {
      const timeLeft = remainingTime || toast.duration;
      this.setTimer(id, timeLeft);
    }
  }

  private setTimer(id: string, duration: number): void {
    const timer = setTimeout(() => {
      this.remove(id);
    }, duration);
    this.timers.set(id, timer);
  }

  private clearTimer(id: string): void {
    if (this.timers.has(id)) {
      clearTimeout(this.timers.get(id));
      this.timers.delete(id);
    }
  }

  private clearAllTimers(): void {
    this.timers.forEach(timer => clearTimeout(timer));
    this.timers.clear();
  }

}

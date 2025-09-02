import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { Toast, ToastService } from '../toast-service/toast-service';

@Component({
  selector: 'app-toast-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast-component.html',
  styleUrls: ['./toast-component.scss']
})
export class ToastComponent implements OnInit, OnDestroy {
  toasts: Toast[] = [];
  private subscription = new Subscription();

  constructor(private toastService: ToastService) {}

  ngOnInit() {
    this.subscription.add(
      this.toastService.toasts$.subscribe(toasts => {
        this.toasts = toasts;
      })
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  getIcon(type: string): string {
    const icons = {
      success: '✓',
      error: '✕',
      warning: '⚠',
      info: 'ℹ',
      default: 'ℹ'
    };
    return icons[type as keyof typeof icons] || icons.default;
  }

  onToastClick(toast: Toast) {
    this.toastService.remove(toast.id);
  }

  onToastMouseEnter(toast: Toast) {
    this.toastService.pauseTimer(toast.id);
  }

  onToastMouseLeave(toast: Toast) {
    this.toastService.resumeTimer(toast.id);
  }

  trackByToastId(index: number, toast: Toast): string {
    return toast.id;
  }
}
